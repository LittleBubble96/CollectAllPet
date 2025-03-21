using System.Collections.Concurrent;
using UnityEngine;

public class UILayerManager
{
    private ConcurrentDictionary<UILayerType,UILayerBase> _layers = new ConcurrentDictionary<UILayerType, UILayerBase>();
    public void Init()
    {
        _layers.TryAdd(UILayerType.Main,new MainUILayer());
        _layers.TryAdd(UILayerType.Pop,new PopUILayer());
        _layers.TryAdd(UILayerType.Tip,new TipsUILayer());
        _layers.TryAdd(UILayerType.Lock,new LockUILayer());
        _layers.TryAdd(UILayerType.Loading,new LoadingUILayer());
    }
    
    public void SetLayerRoot(UILayerType layerType,Transform root)
    {
        UILayerBase layer;
        _layers.TryGetValue(layerType, out layer);
        if (layer != null)
        {
            layer.Init(root);
        }
    }
    
    public UILayerBase GetLayer(UILayerType layerType)
    {
        UILayerBase layer;
        _layers.TryGetValue(layerType, out layer);
        return layer;
    }
}