
public class MainUILayer : UILayerBase
{
    private UIBase _lastUI;
    private UIBase _currentUI;
    public MainUILayer()
    {
        
    }
    
    public override void HandleShowUI(UIManager uiManager,UIBase ui, UIInfo info)
    {
        base.HandleShowUI(uiManager,ui, info);
        if (_currentUI != null)
        {
            _lastUI = _currentUI;
            uiManager.HideUI_UseLayer(_lastUI);
        }
        _currentUI = ui;
    }
    
    public override void HandleHideUI(UIManager uiManager,UIBase ui)
    {
        
    }
    
    public UIBase GetCurrentUI()
    {
        return _currentUI;
    }
    
}