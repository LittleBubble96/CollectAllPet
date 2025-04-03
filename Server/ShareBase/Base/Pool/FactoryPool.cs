
using System;

public class FactoryWithPool<T> where T : IRecycle, new()
{
    protected TypePools<T> _pools;

    public FactoryWithPool()
    { 
        _pools = new TypePools<T>();
    }

    public T GetObject()
    {
        T item = _pools.GetObject<T>();
        return item;
    }

    public void PutObject(T item)
    {
        _pools.PutObject(item);
    }
}

//多类型的池子
public class MutilFactoryWithPool<T> where T : IRecycle, new()
{
    protected MultiPools<T> _pools;

    public MutilFactoryWithPool()
    {
        _pools = new MultiPools<T>();
    }

    public void RegisterType<T1>(Enum type) where T1 : T, new()
    {
        _pools.Register<T1>(type);
    }

    public T GetObject(Enum classType)
    {
        return _pools.GetObject(classType);
    }

    public void PutObject(T item)
    {
        _pools.PutObject(item);
    }
}


//多类型的池子
public class MutilTypeFactoryWithPool<T> where T : IRecycle, new()
{
    protected MultiTypePools<T> _pools;

    public MutilTypeFactoryWithPool()
    {
        _pools = new MultiTypePools<T>();
    }

    public void RegisterType<T1>(Type type) where T1 : T, new()
    {
        _pools.Register<T1>(type);
    }

    public T GetObject(Type classType)
    {
        return _pools.GetObject(classType);
    }

    public void PutObject(T item)
    {
        _pools.PutObject(item);
    }
}