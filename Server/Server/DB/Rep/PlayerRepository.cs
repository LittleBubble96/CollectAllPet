
using MySql.Data.MySqlClient;

public class PlayerRepository
{
    private MySqlDataManager _db;
    
    public void Init(MySqlDataManager db)
    {
        _db = db;
    }
    
    public int CheckPlayerName(string playerName)
    {
        string sql = @"SELECT id FROM player WHERE name = @name";
        var dt = _db.ExecuteQuery(sql, new MySqlParameter("@name", playerName));
        
        if (dt.Rows.Count > 0)
        {
            return (int)dt.Rows[0]["id"];
        }
        
        return -1;
    }
    
    
}