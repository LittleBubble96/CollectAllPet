
public class MySqlUnitWork : IUnitOfWork
{
    private readonly MySqlDataManager _db;
    public MySqlUnitWork(string connectionString)
    {
        _db = new MySqlDataManager(connectionString);
        PlayerRepository = new PlayerRepository(_db);
        PetRepository = new PetRepository(_db);
    }
    
    public void Dispose()
    {
        _db?.Dispose();
    }

    public void Commit()
    {
        // Commit changes to the database
        _db.CommitTransaction();
    }

    public void Rollback()
    {
        // Rollback changes in case of an error
        _db.RollbackTransaction();
    }

    public void BeginTransaction()
    {
        // Begin a new transaction
        _db.BeginTransaction();
    }

    public PlayerRepository PlayerRepository { get; }
    public PetRepository PetRepository { get; }
}