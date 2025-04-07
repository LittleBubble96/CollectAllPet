using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    protected List<CAP_Effect> _effectList = new List<CAP_Effect>();
    protected Dictionary<int, int> _effectDictionary = new Dictionary<int, int>();
    
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
        AddEffectID(effectID, capEffect);
    }
    
    public void StopEffect(int effectID)
    {
        for (int i = _effectList.Count - 1; i >= 0; i--)
        {
            CAP_Effect effect = _effectList[i];
            if (effect.GetEffectID() == effectID)
            {
                DestroyEffect(effect);
                RemoveEffectID(i);
                break;
            }
        }
    }
    
    public void StopAllEffects()
    {
        for (int i = _effectList.Count - 1; i >= 0; i--)
        {
            CAP_Effect effect = _effectList[i];
            DestroyEffect(effect);
            RemoveEffectID(i);
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
            effect.LifeTimeCount -= dt;
            if (effect.LifeTimeCount <= 0)
            {
                DestroyEffect(effect);
                RemoveEffectID(i);
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
    
    protected void AddEffectID(int effectID, CAP_Effect effect)
    {
        _effectList.Add(effect);
        if (!_effectDictionary.ContainsKey(effectID))
        {
            _effectDictionary.Add(effectID,effectID);
        }
    }
    
    protected void RemoveEffectID(int effectIndex)
    {
        CAP_Effect effect = _effectList[effectIndex];
        if (_effectDictionary.ContainsKey(effect.GetEffectID()))
        {
            _effectDictionary.Remove(effect.GetEffectID());
        }
        _effectList.RemoveAt(effectIndex);
    }
    
}