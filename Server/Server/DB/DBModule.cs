
using System.Collections.Concurrent;

public class DBModuleBase
{
    public virtual void Init()
    {
        // Base initialization logic
    }
}

public class DBModule : Singleton<DBModule>
{
    private ConcurrentDictionary<Type, DBModuleBase> m_dbModuleDict = new ConcurrentDictionary<Type, DBModuleBase>();
    private string connectionString = "server=localhost;database=game;uid=root;pwd=CAPCAP;";
    private MySqlUnitWork mysqlUnitWork;

    public void Init()
    {
        mysqlUnitWork = new MySqlUnitWork(connectionString);
        m_dbModuleDict.TryAdd(typeof(CharacterDBService), new CharacterDBService(mysqlUnitWork));
        foreach (var module in m_dbModuleDict)
        {
            module.Value.Init();
        }
    }

    public T GetDbModule<T>() where T : DBModuleBase
    {
        Type type = typeof(T);
        if (m_dbModuleDict.ContainsKey(type))
        {
            return m_dbModuleDict[type] == null ? null : m_dbModuleDict[type] as T;
        }

        return null;
    }
    
    public static T GetDBModule<T>() where T : DBModuleBase
    {
        return Instance.GetDbModule<T>();
    }
}