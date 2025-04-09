
using MySql.Data.MySqlClient;

public class PetRepository
{
    private readonly MySqlDataManager _db;
    public PetRepository(MySqlDataManager db)
    {
        _db = db;
    }
    
    public async Task<List<PetDB>> GetAllPetsAsync(int userId)
    {
        string sql = "SELECT * FROM pet WHERE belongTo = @userId";
        var result = await _db.ExecuteQueryAsync(sql,new MySqlParameter( "@userId", userId));
        List<PetDB> pets = new List<PetDB>();
        for (int i = 0; i < result.Rows.Count; i++)
        {
            var row = result.Rows[i];
            PetDB pet = new PetDB
            {
                Id = (int)row["id"],
                Level = 1,
                BelongTo = (int)row["belongTo"],
                PetConfigId = (int)row["petConfigId"],
                IsBattle = (int)row["isBattle"] == 1,
            };
            pets.Add(pet);
        }

        return pets;
    }
    
    //添加宠物
    public async Task<int> AddPetAsync(int belongTo, int petConfigId, bool isBattle)
    {
        string sql = "INSERT INTO pet (belongTo, petConfigId, isBattle) VALUES (@belongTo, @petConfigId, @isBattle)";
        // excute scalar
        var result = await _db.ExecuteScalarAsync(sql,
            new MySqlParameter("@belongTo", belongTo),
            new MySqlParameter("@petConfigId", petConfigId),
            new MySqlParameter("@isBattle", isBattle ? 1: 0));
        return Convert.ToInt32(result);
    }
}