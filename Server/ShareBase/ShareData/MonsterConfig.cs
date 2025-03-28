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
    }
    public class MonsterConfig
    {
        public static Dictionary<int, MonsterConfigItem> ConfigDict =
            new Dictionary<int, MonsterConfigItem>()
            {
                {
                    1,
                    new MonsterConfigItem()
                        { Id = 1, Name = "Player1", Icon = "Player1", Prefab = "Roke/Monster/Monster1", Desc = "Player1" }
                },
                {
                    2,
                    new MonsterConfigItem()
                        { Id = 2, Name = "Player2", Icon = "Player2", Prefab = "Roke/Monster/Monster2", Desc = "Player2" }
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
}