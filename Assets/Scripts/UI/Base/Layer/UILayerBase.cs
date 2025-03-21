using System.Collections.Generic;
using UnityEngine;

public class UILayerBase
{
    protected Transform m_Root;
    public void Init(Transform root)
    {
        m_Root = root;
    }
    public virtual void HandleShowUI(UIManager uiManager,UIBase ui, UIInfo info)
    {
        ui.transform.SetParent(m_Root, false);
        ui.transform.localPosition = Vector3.zero;
        ui.transform.localScale = Vector3.one;
    }
    
    public virtual void HandleHideUI(UIManager uiManager,UIBase ui)
    {
        
    }
}