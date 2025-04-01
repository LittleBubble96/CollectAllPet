using System.Collections.Generic;
using UnityEngine;

public class BTConfigHelper : Singleton<BTConfigHelper>
{
    protected Dictionary<int, ConfBTCShape> configMap = new Dictionary<int, ConfBTCShape>();
    
    // 获取配置
    public ConfBTCShape GetConfBtcShape(int configKey)
    {
        if (configMap.TryGetValue(configKey, out ConfBTCShape config))
        {
            return config;
        }
        string className = "BT_" + configKey;
        System.Type type = System.Type.GetType(className);
        if (type != null)
        {
            BTCfgBase btCfg = (BTCfgBase)System.Activator.CreateInstance(type);
            ConfBTCShape confBtcShape = btCfg.Root;
            configMap.TryAdd(configKey, confBtcShape);
            return confBtcShape;
        }
        else
        {
            Debug.LogError($"Config class {className} not found.");
            return null;
        }
    }
}