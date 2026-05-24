namespace testProject.Models;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Conference { get; set; } = null!;
    public string Division { get; set; } = null!;
    public ICollection<Player>? Players { get; set; }
}
