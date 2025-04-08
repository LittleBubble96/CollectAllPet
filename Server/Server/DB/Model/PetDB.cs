
public class PetDB
{
    public int PetId { get; set; }
    public int BelongTo { get; set; } // PlayerId
    public int PetConfigId { get; set; }
    public int Level { get; set; }
    public bool IsBattle { get; set; }
    // public List<Equipment> Equipments { get; set; } = new List<Equipment>();
}