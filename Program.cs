using Microsoft.EntityFrameworkCore;
using testProject.Data;
using testProject.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "server=localhost;port=3306;database=HockeyPoolDb;user=root;password=MySqlPassword!";

builder.Services.AddDbContext<HockeyPoolContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HockeyPoolContext>();
    db.Database.EnsureCreated();
    db.Seed();
}

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapGet("/teams", async (HockeyPoolContext db) => await db.Teams.OrderBy(t => t.Name).ToListAsync())
    .WithName("GetTeams");

app.MapGet("/teams/{id}", async (int id, HockeyPoolContext db) =>
    await db.Teams.FindAsync(id) is Team team ? Results.Ok(team) : Results.NotFound())
    .WithName("GetTeamById");

app.MapPost("/teams", async (Team team, HockeyPoolContext db) =>
{
    db.Teams.Add(team);
    await db.SaveChangesAsync();
    return Results.Created($"/teams/{team.Id}", team);
}).WithName("CreateTeam");

app.MapGet("/players", async (HockeyPoolContext db) =>
    await db.Players.Include(p => p.Team).OrderBy(p => p.FullName).ToListAsync())
    .WithName("GetPlayers");

app.MapGet("/players/{id}", async (int id, HockeyPoolContext db) =>
    await db.Players.Include(p => p.Team).FirstOrDefaultAsync(p => p.Id == id) is Player player
        ? Results.Ok(player)
        : Results.NotFound())
    .WithName("GetPlayerById");

app.MapPost("/players", async (Player player, HockeyPoolContext db) =>
{
    db.Players.Add(player);
    await db.SaveChangesAsync();
    return Results.Created($"/players/{player.Id}", player);
}).WithName("CreatePlayer");

app.MapGet("/users", async (HockeyPoolContext db) => await db.Users.OrderBy(u => u.Name).ToListAsync())
    .WithName("GetUsers");

app.MapGet("/users/{id}", async (int id, HockeyPoolContext db) =>
    await db.Users.FindAsync(id) is PoolUser user ? Results.Ok(user) : Results.NotFound())
    .WithName("GetUserById");

app.MapPost("/users", async (PoolUser user, HockeyPoolContext db) =>
{
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/users/{user.Id}", user);
}).WithName("CreateUser");

app.MapGet("/entries", async (HockeyPoolContext db) =>
    await db.Entries
        .Include(e => e.Player)
        .Include(e => e.User)
        .OrderBy(e => e.Week)
        .ThenBy(e => e.Id)
        .ToListAsync())
    .WithName("GetEntries");

app.MapGet("/entries/{id}", async (int id, HockeyPoolContext db) =>
    await db.Entries
        .Include(e => e.Player)
        .Include(e => e.User)
        .FirstOrDefaultAsync(e => e.Id == id) is PoolEntry entry
            ? Results.Ok(entry)
            : Results.NotFound())
    .WithName("GetEntryById");

app.MapPost("/entries", async (PoolEntry entry, HockeyPoolContext db) =>
{
    var userExists = await db.Users.AnyAsync(u => u.Id == entry.UserId);
    var playerExists = await db.Players.AnyAsync(p => p.Id == entry.PlayerId);

    if (!userExists || !playerExists)
    {
        return Results.BadRequest(new { message = "PlayerId and UserId must reference existing records." });
    }

    db.Entries.Add(entry);
    await db.SaveChangesAsync();
    return Results.Created($"/entries/{entry.Id}", entry);
}).WithName("CreateEntry");

app.Run();
