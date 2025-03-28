using System;
using System.Collections.Concurrent;
using System.Text;
using UnityEngine;

public class ActorAnimationParamBase
{
    public string ParamName { get;set; }
    
   
    public virtual bool SetValue(System.ValueType value)
    {
        return false;
    }
    
    public virtual void SetServerValue(string value , Animator animator)
    {
        
    }
    
    public virtual void DoFixedUpdate(Animator animator)
    {
        
    }
}

public class ActorAnimationParamFloat : ActorAnimationParamBase
{
    public float Value { get; set; }

    private float targetValue;
    
    private float currentValue = 0;
    
    public override bool SetValue(System.ValueType value)
    {
        if(value is float f)
        {
            if (Math.Abs(Value - f) > 0.1f)
            {
                Value = f;
                return true;
            }
        }
        return false;
    }
    
    public override void SetServerValue(string value, Animator animator)
    {
        if (value.Contains(GameConst.AnimationPreNameFloat))
        {
            float f = float.Parse(value.Substring(GameConst.AnimationPreNameFloat.Length + 1));
            targetValue = f;
            currentValue = animator.GetFloat(ParamName);
        }
        else
        {
            Debug.LogError("SetServerValue error: " + value);
        }
    }
    
    public override void DoFixedUpdate(Animator animator)
    {
        // 10倍插值  和 character
        currentValue = Mathf.Lerp(currentValue, targetValue, Time.fixedDeltaTime * 10);
        animator.SetFloat(ParamName, currentValue);
        Debug.Log($"DoFixedUpdate: {ParamName} -> {currentValue}");
    }
}

public class ActorAnimationParamInt : ActorAnimationParamBase
{
    public int Value { get; set; }
    
    public override bool SetValue(System.ValueType value)
    {
        if(value is int i)
        {
            if (Value != i)
            {
                Value = i;
                return true;
            }
        }
        return false;
    }
    
    public override void SetServerValue(string value, Animator animator)
    {
        if (value.Contains(GameConst.AnimationPreNameInt))
        {
            int i = int.Parse(value.Substring(GameConst.AnimationPreNameInt.Length + 1));
            animator.SetInteger(ParamName, i);
        }
        else
        {
            Debug.LogError("SetServerValue error: " + value);
        }
    }
}

public class ActorAnimationParamBool : ActorAnimationParamBase
{
    public bool Value { get; set; }
    
    public override bool SetValue(System.ValueType value)
    {
        if(value is bool b)
        {
            if (Value != b)
            {
                Value = b;
                return true;
            }
        }
        return false;
    }
    
    public override void SetServerValue(string value, Animator animator)
    {
        if (value.Contains(GameConst.AnimationPreNameBool))
        {
            string s = value.Substring(GameConst.AnimationPreNameBool.Length + 1);
            bool b = s == "1";
            animator.SetBool(ParamName, b);
        }
        else
        {
            Debug.LogError("SetServerValue error: " + value);
        }
    }
}




public class ActorAnimationController
{
    public ConcurrentDictionary<string, ActorAnimationParamBase> AnimationParams {get; private set;}
    
    public Animator Animator { get; set; }
    
    public Action<string,float> OnSetFloat;
    public Action<string,int> OnSetInt;
    public Action<string,bool> OnSetBool;
    
    public void Init(Animator animator)
    {
        AnimationParams = new ConcurrentDictionary<string, ActorAnimationParamBase>();
        Animator = animator;
    }
    
    public void DoFixedUpdate()
    {
        if (Animator == null)
        {
            return;
        }
        foreach (var param in AnimationParams)
        {
            param.Value.DoFixedUpdate(Animator);
        }
    }
    
    public void SetFloat(string paramName, float value)
    {
        bool isChanged = true;
        if(AnimationParams.ContainsKey(paramName))
        {
            isChanged = AnimationParams[paramName].SetValue(value);
        }
        else
        {
            AnimationParams.TryAdd(paramName, new ActorAnimationParamFloat(){ParamName = paramName, Value = value});
        }
        if (isChanged)
        {
            OnSetFloat?.Invoke(paramName, value);
        }
    }
    
    public void SetInt(string paramName, int value)
    {
        bool isChanged = true;
        if(AnimationParams.ContainsKey(paramName))
        {
            isChanged = AnimationParams[paramName].SetValue(value);
        }
        else
        {
            AnimationParams.TryAdd(paramName, new ActorAnimationParamInt(){ParamName = paramName, Value = value});
        }
        if (isChanged)
        {
            OnSetInt?.Invoke(paramName, value);
        }
    }
    
    public void SetBool(string paramName, bool value)
    {
        bool isChanged = true;
        if(AnimationParams.ContainsKey(paramName))
        {
            isChanged = AnimationParams[paramName].SetValue(value);
        }
        else
        {
            AnimationParams.TryAdd(paramName, new ActorAnimationParamBool(){ParamName = paramName, Value = value});
        }
        if (isChanged)
        {
            OnSetBool?.Invoke(paramName, value);
        }
    }
    
    // Server
    public void SetServerAnimationParam(string paramName, string value)
    {
        if (Animator == null)
        {
            return;
        }
        Debug.Log($"SetServerAnimationParam: {paramName} -> {value}");
        // value format: float:1.0f,int:1,bool:true,string:xxx
        if (!AnimationParams.ContainsKey(paramName))
        {
            if (value.Contains(GameConst.AnimationPreNameFloat))
            {
                AnimationParams.TryAdd(paramName, new ActorAnimationParamFloat(){ParamName = paramName});
            }
            else if (value.Contains(GameConst.AnimationPreNameInt))
            {
                AnimationParams.TryAdd(paramName, new ActorAnimationParamInt(){ParamName = paramName});
            }
            else if (value.Contains(GameConst.AnimationPreNameBool))
            {
                AnimationParams.TryAdd(paramName, new ActorAnimationParamBool(){ParamName = paramName});
            }
        }
        AnimationParams[paramName].SetServerValue(value, Animator);
    }
}