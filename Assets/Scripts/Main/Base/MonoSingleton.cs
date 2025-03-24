using UnityEngine;

public class MonoSingleton : MonoBehaviour
{
    private static MonoSingleton _instance;
    private object _lock = new object();

    public static MonoSingleton Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_instance)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<MonoSingleton>();
                        if (FindObjectsOfType<MonoSingleton>().Length > 1)
                        {
                            return _instance;
                        }
                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            _instance = singleton.AddComponent<MonoSingleton>();
                            singleton.name = "(singleton) " + typeof(MonoSingleton).ToString();
                            DontDestroyOnLoad(singleton);
                        }
                    }
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as MonoSingleton;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}