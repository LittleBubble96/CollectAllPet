using System.Collections.Generic;

namespace ShareProtobuf.ShareData
{
    public class PlayerConfigItem
    {
        public int Id;
        public string Name;
        public string Icon;
        public string Prefab;
        public string Desc;
    }
    public class PlayerConfig
    {
        public static Dictionary<int, PlayerConfigItem> PlayerConfigDict =
            new Dictionary<int, PlayerConfigItem>()
            {
                {
                    1,
                    new PlayerConfigItem()
                        { Id = 1, Name = "Player1", Icon = "Player1", Prefab = "Role/Character", Desc = "Player1" }
                },
                {
                    2,
                    new PlayerConfigItem()
                        { Id = 2, Name = "Player2", Icon = "Player2", Prefab = "Player2", Desc = "Player2" }
                }
            };

        public static PlayerConfigItem GetPlayerConfigItem(int playerCfgId)
        {
            if (PlayerConfigDict.ContainsKey(playerCfgId))
            {
                return PlayerConfigDict[playerCfgId];
            }

            return null;
        }


    }
}