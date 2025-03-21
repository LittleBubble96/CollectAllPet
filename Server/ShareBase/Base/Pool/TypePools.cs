using System;
using System.Collections.Concurrent;
public interface IRecycle 
{
    void Recycle();
}

public class Pool<T> where T : IRecycle , new()
{
    private ConcurrentBag<T> _objects;
    private Type _type;

    public Pool()
    {
        _objects = new ConcurrentBag<T>();
    }
    
    public Pool(Type type)
    {
        _objects = new ConcurrentBag<T>();
        _type = type;
    }

    public T GetObject()
    {
        T item;
        if (_objects.TryTake(out item))
            return item;
        if (_type == null)
        {
            return new T();
        }
        return (T)Activator.CreateInstance(_type);
    }

    public void PutObject(T item)
    {
        _objects.Add(item);
        item.Recycle();
    }
}


public class TypePools<T>  where T : IRecycle, new()
{
    protected ConcurrentDictionary<Type, Pool<T>> _pools;

    public TypePools()
    {
        _pools = new ConcurrentDictionary<Type, Pool<T>>();
    }

    public T GetObject<T1>() where T1 : T, new()
    {
        Pool<T> pool;
        if (_pools.TryGetValue(typeof(T), out pool))
            return pool.GetObject();
        if (pool == null)
            pool = _pools.GetOrAdd(typeof(T), new Pool<T>());
        return pool.GetObject();
    }

    public void PutObject(T item)
    {
        Pool<T> pool;
        if (_pools.TryGetValue(item.GetType(), out pool))
            pool.PutObject(item);
    }
}


//多类型的池子

public class MultiPools<T> where T : IRecycle, new()
{ 
    protected ConcurrentDictionary<Enum, Pool<T>> _pools;
    protected ConcurrentDictionary<Type, Enum> _typeEnums;
    public MultiPools() {
        _pools = new ConcurrentDictionary<Enum, Pool<T>>();
        _typeEnums = new ConcurrentDictionary<Type, Enum>();
    }

    public void Register<T1>(Enum type) where T1 : T , new()
    {
        if (_typeEnums.ContainsKey(typeof(T1)))
        { 
            throw new Exception("Type already registered");
        }
        if (!_pools.TryGetValue(type, out var pool))
        { 
            pool = new Pool<T>(typeof(T1));
            _pools.TryAdd(type, pool);
        }
        _typeEnums.TryAdd(typeof(T1), type);
    }

    public T GetObject(Enum type) {
        Pool<T> pool;
        if (_pools.TryGetValue(type, out pool))
            return pool.GetObject();
        if (pool == null)
            pool = _pools.GetOrAdd(type, new Pool<T>());
        return pool.GetObject();
    }

    public void PutObject(T item)
    {
        Pool<T> pool;
        Enum classType;
        if (_typeEnums.TryGetValue(typeof(T), out classType))
        {
            if (_pools.TryGetValue(classType, out pool))
                pool.PutObject(item);
        }
    }
}


