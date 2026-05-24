namespace testProject.Models;

public class Player
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Position { get; set; } = null!;
    public int TeamId { get; set; }
    public Team? Team { get; set; }
    public ICollection<PoolEntry>? Entries { get; set; }
}
