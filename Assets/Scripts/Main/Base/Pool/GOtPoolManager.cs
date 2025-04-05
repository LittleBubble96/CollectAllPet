using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class GOtPoolManager : Singleton<GOtPoolManager>
{
    private ConcurrentDictionary<string, Stack<RecycleObject>> _poolDictionary = new ConcurrentDictionary<string, Stack<RecycleObject>>();
    
    public void Init()
    {
        // Initialize the pool manager
    }
    
    public T Get<T>(string prefabName) where T : RecycleObject
    {
        RecycleObject recycleObject = null;
        if (_poolDictionary.TryGetValue(prefabName, out Stack<RecycleObject> pool))
        {
            if (pool.Count > 0)
            {
                recycleObject = pool.Pop();
            }
        }
        if (recycleObject == null)
        {
            GameObject prefab = Resources.Load<GameObject>(prefabName);
            if (prefab != null)
            {
                GameObject instance = Object.Instantiate(prefab);
                recycleObject = instance.GetComponent<RecycleObject>();
                if (recycleObject == null)
                {
                    Debug.LogError($"Prefab {prefabName} does not have a RecycleObject component.");
                    return null;
                }
            }
            else
            {
                Debug.LogError($"Prefab {prefabName} not found.");
                return null;
            }
        }
        recycleObject.ObjectName = prefabName;
        recycleObject.gameObject.SetActive(true);
        recycleObject.transform.SetParent(null);
        return recycleObject as T;
    }
    
    public void Return(RecycleObject obj)
    {
        string prefabName = obj.ObjectName;
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(null);
        obj.Recycle();
        if (_poolDictionary.TryGetValue(prefabName, out Stack<RecycleObject> pool))
        {
            pool.Push(obj);
        }
        else
        {
            Stack<RecycleObject> newPool = new Stack<RecycleObject>();
            newPool.Push(obj);
            _poolDictionary[prefabName] = newPool;
        }
    }
    
}