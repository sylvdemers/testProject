using System.Linq;
using Microsoft.EntityFrameworkCore;
using testProject.Models;

namespace testProject.Data;

public class HockeyPoolContext : DbContext
{
    public HockeyPoolContext(DbContextOptions<HockeyPoolContext> options) : base(options)
    {
    }

    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Player> Players => Set<Player>();
    public DbSet<PoolUser> Users => Set<PoolUser>();
    public DbSet<PoolEntry> Entries => Set<PoolEntry>();

    public void Seed()
    {
        if (Teams.Any())
        {
            return;
        }

        var redWings = new Team { Name = "Red Wings", City = "Detroit", Conference = "Eastern", Division = "Atlantic" };
        var bruins = new Team { Name = "Bruins", City = "Boston", Conference = "Eastern", Division = "Atlantic" };
        var mapleLeafs = new Team { Name = "Maple Leafs", City = "Toronto", Conference = "Eastern", Division = "Atlantic" };

        Teams.AddRange(redWings, bruins, mapleLeafs);
        SaveChanges();

        var players = new[]
        {
            new Player { FullName = "Connor McDavid", Position = "C", TeamId = redWings.Id },
            new Player { FullName = "David Pastrnak", Position = "RW", TeamId = bruins.Id },
            new Player { FullName = "Auston Matthews", Position = "C", TeamId = mapleLeafs.Id }
        };

        Players.AddRange(players);

        var users = new[]
        {
            new PoolUser { Name = "Alex Tremblay", Email = "alex@example.com" },
            new PoolUser { Name = "Julie Martin", Email = "julie@example.com" }
        };

        Users.AddRange(users);
        SaveChanges();

        Entries.AddRange(
            new PoolEntry { UserId = users[0].Id, PlayerId = players[0].Id, Week = 1, Points = 18, Notes = "Capitaine du premier pool" },
            new PoolEntry { UserId = users[1].Id, PlayerId = players[1].Id, Week = 1, Points = 14, Notes = "Choix de l’équipe" }
        );

        SaveChanges();
    }
}
