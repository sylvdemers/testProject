namespace testProject.Models;

public class PoolEntry
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public PoolUser? User { get; set; }
    public int PlayerId { get; set; }
    public Player? Player { get; set; }
    public int Week { get; set; }
    public int? Points { get; set; }
    public string? Notes { get; set; }
}
