
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
        public string HitEffectName;
        public int[] GenGoldRandoms;
        //收获钻石概率 0.0-1.0
        public float GenDiamondProbability;
        //获取钻石随机数
        public int[] GenDiamondRandoms;
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
                            DestroyEffectName ="Effect/BreakSmoke", HitEffectName = "Effect/FireHit",
                            GenGoldRandoms = new []{300,400}, GenDiamondProbability = 0.5f, GenDiamondRandoms = new []{2,8},
                        }
                },
                {
                    2,
                    new BreakInteractiveItem()
                        { Id = 2, Name = "BreakInteractive2", Prefab = "Scene/BreakInteractive_Ice", HP = 10  ,
                            DestroyEffectName ="Effect/BreakSmoke1", HitEffectName = "Effect/IceHit",
                            GenGoldRandoms = new []{300,400}, GenDiamondProbability = 0.5f, GenDiamondRandoms = new []{2,8},
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