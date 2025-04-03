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
                        { Id = 1, Name = "Player1", Icon = "Player1", Prefab = "Role/Pet/Pet1", Desc = "Player1" ,AiId = 1001 , 
                            AttackRange = 2f ,CancelAttackRange = 5f,AttackInterval = 2f,AttackDamage = 1}
                },
                {
                    2,
                    new MonsterConfigItem()
                        { Id = 2, Name = "Player2", Icon = "Player2", Prefab = "Role/Pet/Pet2", Desc = "Player2" ,AiId = 1001 , 
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
}