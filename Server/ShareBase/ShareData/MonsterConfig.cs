using System.Collections.Generic;

namespace ShareProtobuf.ShareData
{
    public class MonsterConfigItem
    {
        public int Id;
        public string Name;
        public string Icon;
        public string Prefab;
        public string Desc;
        public int AiId;
        //质量
        public EPetQuality Quality;
        //攻击伤害
        public int AttackDamage;
        //攻击距离
        public float AttackRange;
        //取消攻击距离
        public float CancelAttackRange;
        //攻击间隔
        public float AttackInterval;
    }
    public class MonsterConfig
    {
        public static Dictionary<int, MonsterConfigItem> ConfigDict =
            new Dictionary<int, MonsterConfigItem>()
            {
                {
                    1,
                    new MonsterConfigItem()
                        { Id = 1, Name = "Pet1", Icon = "Pet1", Prefab = "Role/Pet/Pet1", 
                            Quality = EPetQuality.White, Desc = "Pet1" ,AiId = 1001 , 
                            AttackRange = 2f ,CancelAttackRange = 5f,AttackInterval = 2f,AttackDamage = 1}
                },
                {
                    2,
                    new MonsterConfigItem()
                        { Id = 2, Name = "Pet2", Icon = "Pet2", Prefab = "Role/Pet/Pet2", 
                            Quality = EPetQuality.White,Desc = "Pet2" ,AiId = 1001 , 
                            AttackRange = 2f ,CancelAttackRange = 5f,AttackInterval = 3f,AttackDamage = 1}
                }
            };

        public static MonsterConfigItem GetConfigItem(int CfgId)
        {
            if (ConfigDict.ContainsKey(CfgId))
            {
                return ConfigDict[CfgId];
            }

            return null;
        }

        #region 质量

        private static Dictionary<EPetQuality, List<MonsterConfigItem>> qualityDict;

        //获取质量 对应的怪物列表
        public static Dictionary<EPetQuality,List<MonsterConfigItem>> GetQualityDict()
        {
            if (qualityDict == null)
            {
                qualityDict = new Dictionary<EPetQuality, List<MonsterConfigItem>>();
                foreach (var item in ConfigDict.Values)
                {
                    if (!qualityDict.ContainsKey(item.Quality))
                    {
                        qualityDict[item.Quality] = new List<MonsterConfigItem>();
                    }
                    qualityDict[item.Quality].Add(item);
                }
            }

            return qualityDict;
        }

        #endregion
    }
    
    public enum EMonsterAttribute
    {
        All = -1,
        None = 0,
        AttackRange = 1<<0,
        AttackInterval = 1<<1,
        CancelAttackRange = 1<<2,
        AttackDamage = 1<<3,
    }
    
    public enum EPetQuality
    {
        None = 0,
        White = 1,
        Green = 2,
        Blue = 3,
        //紫色
        Purple = 4,
        //金色
        Golden = 5,
        //红色
        Red = 6,
    }
}