
using System;

// public class FactoryWithPool<T> where T : IRecycle, new()
// {
//     protected TypePools<T> _pools;
//
//     public FactoryWithPool()
//     { 
//         _pools = new TypePools<T>();
//     }
//
//     public T1 GetObject<T1>() where T1 : T, new()
//     {
//         T item = _pools.GetObject<T1>();
//         return (T1)item;
//     }
//
//     public void PutObject(T item)
//     {
//         _pools.PutObject(item);
//     }
// }

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