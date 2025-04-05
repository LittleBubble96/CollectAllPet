using UnityEngine;

public class CAP_Effect : RecycleObject
{ 
    [SerializeField] private float lifeTime = 1f;
    
    protected int EffectID;
    
    protected bool isLoop = false;
    
    public void Init(int effectID , bool loop = false)
    {
        EffectID = effectID;
        isLoop = loop;
    }
    
    public float LifeTime
    {
        get => lifeTime;
        set => lifeTime = value;
    }
    
    public int GetEffectID()
    {
        return EffectID;
    }
    
    public bool IsLoop()
    {
        return isLoop;
    }
}