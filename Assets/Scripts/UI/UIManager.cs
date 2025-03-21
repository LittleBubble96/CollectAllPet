using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum UILayerType
{
    Main,
    Pop,
    Tip,
    Loading,
    Lock,
}

public class UIManager
{
    private Canvas _canvas;
    
    private ConcurrentDictionary<Type,ConcurrentQueue<UIBase>> _uiPool;
    private List<UIBase> _uiList;
    private UIBase _lockUI;
    
    protected UILayerManager _layerManager;
    
    private string _canvasPath = "UI/Canvas";

    public void Init()
    {
        _uiPool = new ConcurrentDictionary<Type, ConcurrentQueue<UIBase>>();
        _uiList = new List<UIBase>();
        _layerManager = new UILayerManager();
        _layerManager.Init();
        CreateCanvas();
    }
    
    public void DoUpdate(float dt)
    {
        
        for (int i = _uiList.Count - 1; i >= 0; i--)
        {
            UIBase ui = _uiList.ElementAt(i);
            ui.UpdateUI(dt);
            // if (!pendingKill)
            // {
            //     _uiList.RemoveAt(i);
            // }
        }
    }

    public T ShowUI<T> () where T : UIBase
    {
       UIInfo uiInfo = FindUiInfo<T>();
       UIBase ui = ActiveUI<T>(uiInfo);
       UILayerBase layer = _layerManager.GetLayer(ui.GetInfo().LayerType);
       layer.HandleShowUI(this,ui,ui.GetInfo());
       ShowUI_Internal(ui);
       return ui as T;
    }
    
    public void HideUI(UIBase ui)
    {
        UILayerBase layer = _layerManager.GetLayer(ui.GetInfo().LayerType);
        layer.HandleHideUI(this,ui);
        DeActiveUI(ui);
        HideUI_Internal(ui);
    }
    
    //后续可以考虑优化 namespace
    public void HideUI_UseLayer(UIBase ui)
    {
        UILayerBase layer = _layerManager.GetLayer(ui.GetInfo().LayerType);
        layer.HandleHideUI(this,ui);
        DeActiveUI(ui);
        HideUI_Internal(ui);
    }
    
    public void ShowLockUI()
    {
        UIInfo uiInfo = FindUiInfo<Lock_UI>();
        if (!_lockUI)
        {
            _lockUI = CreateUI(uiInfo);
        }
        _lockUI.gameObject.SetActive(true);
    }
    
    public void HideLockUI()
    {
        if (_lockUI)
        {
            _lockUI.gameObject.SetActive(false);
        }
    }
    
    public void ShowLoadingUI()
    {
        UIInfo uiInfo = FindUiInfo<Loading_UI>();
        UIBase ui = ActiveUI<Loading_UI>(uiInfo);
        UILayerBase layer = _layerManager.GetLayer(ui.GetInfo().LayerType);
        layer.HandleShowUI(this,ui,ui.GetInfo());
        ShowUI_Internal(ui);
    }
    
    protected void CreateCanvas()
    {
        GameObject res = Resources.Load<GameObject>(_canvasPath);
        GameObject go = GameObject.Instantiate(res);
        _canvas = go.GetComponent<Canvas>();
        Transform mainLayer = go.transform.Find("MainLayer");
        Transform popLayer = go.transform.Find("PopLayer");
        Transform tipLayer = go.transform.Find("TipLayer");
        Transform loadingLayer = go.transform.Find("LoadingLayer");
        Transform lockLayer = go.transform.Find("LockLayer");
        _layerManager.SetLayerRoot(UILayerType.Main,mainLayer);
        _layerManager.SetLayerRoot(UILayerType.Pop,popLayer);
        _layerManager.SetLayerRoot(UILayerType.Tip,tipLayer);
        _layerManager.SetLayerRoot(UILayerType.Loading,loadingLayer);
        _layerManager.SetLayerRoot(UILayerType.Lock,lockLayer);
    }
    
    protected UIBase ActiveUI<T>(UIInfo uiInfo)
    {
        Type type = typeof(T);
        if (_uiPool.TryGetValue(type, out ConcurrentQueue<UIBase> queue))
        {
            if (queue.TryDequeue(out UIBase ui))
            {
                return ui;
            }
        }
        UIBase uiBase = CreateUI(uiInfo);
        return uiBase;
    }
    
    protected void DeActiveUI(UIBase ui)
    {
        Type type = ui.GetType();
        ConcurrentQueue<UIBase> queue = _uiPool.GetOrAdd(type, new ConcurrentQueue<UIBase>());
        queue.Enqueue(ui);
    }
    
    protected UIBase CreateUI(UIInfo uiInfo)
    {
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(uiInfo.ResPath), _canvas.transform);
        UIBase uiBase = go.GetComponent<UIBase>();
        uiBase.Init(uiInfo);
        return uiBase;
    }

    protected UIInfo FindUiInfo<T>()
    {
        Type type = typeof(T);
        if (RegisterUIHelper.UIInfos.TryGetValue(type, out UIInfo uiInfo))
        {
            return uiInfo;
        }
        return new UIInfo();
    }
    
    protected void ShowUI_Internal(UIBase ui)
    {
       ui.gameObject.SetActive(true);
       ui.transform.SetAsLastSibling();
       
       this._uiList.Add(ui);
       ui.OnShow();
    }
    
    protected void HideUI_Internal(UIBase ui)
    {
        ui.gameObject.SetActive(false);
        lock (_uiList)
        {
            this._uiList.Remove(ui);
        }
        ui.SetPendingDestroy();
        ui.OnHide();
    }
}