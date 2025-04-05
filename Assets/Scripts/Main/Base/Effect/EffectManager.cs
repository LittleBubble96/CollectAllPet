using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    protected List<CAP_Effect> _effectList = new List<CAP_Effect>();
    
    private int _effectID = 0;
    private int _maxEffectCount = 1000;
    public void Init()
    {
        // Initialize the effect manager
    }
    
    public void PlayEffect(string effectName, Vector3 position, Quaternion rotation,bool isLoop = false)
    {
        CAP_Effect capEffect = GOtPoolManager.Instance.Get<CAP_Effect>(effectName);
        capEffect.transform.position = position;
        capEffect.transform.rotation = rotation;
        int effectID = GetGenerateEffectId();
        capEffect.Init(effectID, isLoop);
        _effectList.Add(capEffect);
    }
    
    public void StopEffect(int effectID)
    {
        for (int i = _effectList.Count - 1; i >= 0; i--)
        {
            CAP_Effect effect = _effectList[i];
            if (effect.GetEffectID() == effectID)
            {
                DestroyEffect(effect);
                _effectList.RemoveAt(i);
                break;
            }
        }
    }
    
    public void DoUpdate(float dt)
    {
        for (int i = _effectList.Count - 1; i >= 0; i--)
        {
            CAP_Effect effect = _effectList[i];
            if (effect.IsLoop())
            {
                continue;
            }
            effect.LifeTime -= dt;
            if (effect.LifeTime <= 0)
            {
                DestroyEffect(effect);
                _effectList.RemoveAt(i);
            }
        }
    }
    
    protected void DestroyEffect(CAP_Effect effect)
    {
        if (effect != null)
        {
            GOtPoolManager.Instance.Return(effect);
        }
    }
    
    protected int GetGenerateEffectId()
    {
        _effectID++;
        if (_effectID >= _maxEffectCount)
        {
            _effectID = 0;
        }
        while (_effectDictionary.ContainsKey(_effectID))
        {
            _effectID++;
            if (_effectID >= _maxEffectCount)
            {
                _effectID = 0;
            }
        }
        return _effectID;
    }
    
}