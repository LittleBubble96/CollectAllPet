
public class PlayerDB
{
    public int PlayerId { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public long Exp { get; set; }
    public DateTime LastLoginTime { get; set; }
    public List<PetDB> Pets { get; set; } = new List<PetDB>();
}