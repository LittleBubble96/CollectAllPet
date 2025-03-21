using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class PopUILayer : UILayerBase
{
    private ConcurrentStack<UIBase> _uiStack;
    
    public PopUILayer()
    {
        _uiStack = new ConcurrentStack<UIBase>();
    }
    public override void HandleShowUI(UIManager uiManager, UIBase ui, UIInfo info)
    {
        base.HandleShowUI(uiManager,ui, info);
        _uiStack.Push(ui);
    }
    
    public override void HandleHideUI(UIManager uiManager,UIBase ui)
    {
        _uiStack.TryPop(out UIBase popUI);
        while (popUI != ui)
        {
            uiManager.HideUI_UseLayer(popUI);
            _uiStack.TryPop(out popUI);
        }
    }
}