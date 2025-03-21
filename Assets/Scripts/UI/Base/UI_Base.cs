using UnityEngine;

public struct UIInfo
{
    public string ResPath;
    public UILayerType LayerType;
}

public class UIBase : MonoBehaviour
{
    private UIInfo _info;
    private bool bPendingDestroy = false;
    
    public void Init(UIInfo info)
    {
        _info = info;
        bPendingDestroy = false;
        OnInit();
    }
    
    public UIInfo GetInfo()
    {
        return _info;
    }

    public virtual void OnInit()
    {
        
    }

    public virtual void OnShow()
    {
        
    }
    
    public virtual void OnHide()
    {
        
    }

    public void Hide()
    {
        GameManager.GetUIManager().HideUI(this);
    }

    public void UpdateUI(float dt)
    {
        if (bPendingDestroy)
        {
            return;
        }
        DoUpdate(dt);
        return;
    
    }

    public virtual void DoUpdate(float dt)
    {
        
    }
    
    public void SetPendingDestroy()
    {
        bPendingDestroy = true;
    }
}