
using System.Collections.Generic;

namespace ShareProtobuf.ShareData
{
    public class BreakInteractiveItem
    {
        public int Id;
        public string Name;
        public string Prefab;
        public float HP;
        public string DestroyEffectName;
        public Vector3 DestroyEffectOffset;
        public string HitEffectName;
        public Vector3 HitEffectOffset;
    }
    public class BreakInteractiveConfig
    {
        public static Dictionary<int, BreakInteractiveItem> ConfigDict =
            new Dictionary<int, BreakInteractiveItem>()
            {
                {
                    1,
                    new BreakInteractiveItem()
                        { Id = 1, Name = "BreakInteractive1", Prefab = "Scene/BreakInteractive_fire", HP = 10 ,
                            DestroyEffectName ="Effect/BreakSmoke",DestroyEffectOffset = new Vector3(0,0,0),
                            HitEffectName = "Effect/FireHit",HitEffectOffset = new Vector3(0,0.5f,0.5f)
                        }
                },
                {
                    2,
                    new BreakInteractiveItem()
                        { Id = 2, Name = "BreakInteractive2", Prefab = "Scene/BreakInteractive_Ice", HP = 10  ,
                            DestroyEffectName ="Effect/BreakSmoke1",DestroyEffectOffset = new Vector3(0,0,0),
                            HitEffectName = "Effect/IceHit",HitEffectOffset = new Vector3(0,0.5f,0.5f)
                        }
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