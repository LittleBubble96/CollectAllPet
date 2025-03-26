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
                        { Id = 1, Name = "Player1", Icon = "Player1", Prefab = "Role/Character", Desc = "Player1" }
                },
                {
                    2,
                    new MonsterConfigItem()
                        { Id = 2, Name = "Player2", Icon = "Player2", Prefab = "Player2", Desc = "Player2" }
                }
            };
    }
}