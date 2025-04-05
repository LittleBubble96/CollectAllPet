

using System.Collections.Generic;

namespace ShareProtobuf.ShareData
{
    public class ScenePointConfigItem
    {
        public int Id;
        public string Name;
        public int[] BreakInteractiveIds;
        public float RandomRadius;
        public Vector3 Position;
        public Vector3 Rotation;
        public float ReSpawnTime;
    }
    public class ScenePointConfig
    {
        public static Dictionary<int, ScenePointConfigItem> ConfigDict =
            new Dictionary<int, ScenePointConfigItem>()
            {
                {
                    1,
                    new ScenePointConfigItem()
                        { Id = 1, Name = "Point1", BreakInteractiveIds =new []{1,2}, 
                            RandomRadius = 1f,Position = new Vector3(70, 21.947f, 31.09f), Rotation = new Vector3() ,
                            ReSpawnTime = 3f,
                        }
                },
                {
                    2,
                    new ScenePointConfigItem()
                    {
                        Id = 2, Name = "Point2", BreakInteractiveIds =new []{1,2}, RandomRadius = 1f, Position = new Vector3(82.22f, 21.51f, 31.09f), Rotation = new Vector3() ,
                        ReSpawnTime = 3f,
                    }
                }
            };

        public static ScenePointConfigItem GetConfigItem(int cfgId)
        {
            if (ConfigDict.ContainsKey(cfgId))
            {
                return ConfigDict[cfgId];
            }

            return null;
        }


    }
}