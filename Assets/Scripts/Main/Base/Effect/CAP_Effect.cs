using UnityEngine;

public class CAP_Effect : RecycleObject
{ 
    [SerializeField] private float lifeTime = 1f;
    
    private float lifeTimeCount = 0f;
    
    protected int EffectID;
    
    protected bool isLoop = false;
    
    public void Init(int effectID , bool loop = false)
    {
        EffectID = effectID;
        isLoop = loop;
        LifeTimeCount = LifeTime;
    }
    
    public float LifeTime
    {
        get => lifeTime;
        set => lifeTime = value;
    }
    
    public float LifeTimeCount
    {
        get => lifeTimeCount;
        set => lifeTimeCount = value;
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