namespace testProject.Models;

public class PoolUser
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public ICollection<PoolEntry>? Entries { get; set; }
}
