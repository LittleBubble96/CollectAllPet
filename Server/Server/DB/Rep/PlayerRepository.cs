
using MySql.Data.MySqlClient;

public class PlayerRepository
{
    private MySqlDataManager _db;

    public PlayerRepository(MySqlDataManager db)
    {
        _db = db;
    }

    public async Task<int> CheckPlayerName(string username)
    {
        string sql = @"SELECT id FROM player WHERE username = @username";
        var dt = await _db.ExecuteQueryAsync(sql, new MySqlParameter("@username", username));
        
        if (dt.Rows.Count > 0)
        {
            return (int)dt.Rows[0]["id"];
        }
        
        return -1;
    }
    
    public async Task<int> CreatePlayer(string username, string password)
    {
        string sql = @"INSERT INTO player (username, password) VALUES (@username, @password)";
        await _db.ExecuteQueryAsync(sql, new MySqlParameter("@username", username), new MySqlParameter("@password", password));
        return await CheckPlayerName(username);
    }

    public async Task<bool> CheckPlayerPassword(string username, string password)
    {
        string sql = @"SELECT id FROM player WHERE username = @username AND password = @password";
        var dt = await _db.ExecuteQueryAsync(sql, new MySqlParameter("@username", username), new MySqlParameter("@password", password));
        
        if (dt.Rows.Count > 0)
        {
            return true;
        }
        
        return false;
    }

    
    public async Task<bool> CheckUid(int uid)
    {
        string sql = @"SELECT id FROM player WHERE id = @uid";
        var dt = await _db.ExecuteQueryAsync(sql, new MySqlParameter("@uid", uid));
        
        if (dt.Rows.Count > 0)
        {
            return true;
        }
        
        return false;
    }
    
    public async Task<PlayerDB> GetPlayerInfo(int uid)
    {
        string sql = @"SELECT * FROM player WHERE id = @uid";
        var dt = await _db.ExecuteQueryAsync(sql, new MySqlParameter("@uid", uid));
        
        if (dt.Rows.Count > 0)
        {
            PlayerDB player = new PlayerDB();
            player.PlayerId = (int)dt.Rows[0]["id"];
            player.Name = dt.Rows[0]["username"].ToString();
            player.Exp = 0;
            player.Level = 1;
            return player;
        }
        return null;
    }

}