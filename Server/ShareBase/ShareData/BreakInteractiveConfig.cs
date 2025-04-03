
using System.Collections.Generic;

namespace ShareProtobuf.ShareData
{
    public class BreakInteractiveItem
    {
        public int Id;
        public string Name;
        public string Prefab;
        public float HP;
    }
    public class BreakInteractiveConfig
    {
        public static Dictionary<int, BreakInteractiveItem> ConfigDict =
            new Dictionary<int, BreakInteractiveItem>()
            {
                {
                    1,
                    new BreakInteractiveItem()
                        { Id = 1, Name = "BreakInteractive1", Prefab = "Scene/BreakInteractive", HP = 100}
                },
                {
                    2,
                    new BreakInteractiveItem()
                        { Id = 2, Name = "BreakInteractive2", Prefab = "Scene/BreakInteractive1", HP = 100  }
                }
            };

        public static BreakInteractiveItem GetConfigItem(int cfgId)
        {
            if (ConfigDict.ContainsKey(cfgId))
            {
                return ConfigDict[cfgId];
            }

            return null;
        }


    }
    
    public enum EBreakInteractiveAttribute
    {
        None = 0,
        All = -1,
        Hp = 1<<0,
    }
}