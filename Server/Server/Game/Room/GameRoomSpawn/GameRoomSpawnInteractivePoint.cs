
using ShareProtobuf;
using ShareProtobuf.ShareData;

public enum SpawnInteractiveState
{
    None = 0,
    WaitSpawn = 1,
    Spawn = 2,
    Playing = 3,
    WaitDestroy = 4,
    Destroy = 5,
}
public class GameRoomSpawnInteractivePoint
{
    public ScenePointConfigItem ScenePointConfigItem { get; set; }

    private SpawnInteractiveState spawnInteractiveState;
    public SpawnInteractiveState SpawnInteractiveState
    {
        get { return spawnInteractiveState; }
        set
        {
            if (spawnInteractiveState == value) return;
            spawnInteractiveState = value;
            switch (spawnInteractiveState)
            {
                case SpawnInteractiveState.None:
                    break;
                case SpawnInteractiveState.WaitSpawn:
                    OnWaitSpawn();
                    break;
                case SpawnInteractiveState.Spawn:
                    break;
                case SpawnInteractiveState.Playing:
                    break;
                case SpawnInteractiveState.WaitDestroy:
                    break;
                case SpawnInteractiveState.Destroy:
                    break;
            }
        }
    }
    
    public void Init( ScenePointConfigItem scenePointConfigItem)
    {
        ScenePointConfigItem = scenePointConfigItem;
        SpawnInteractiveState = SpawnInteractiveState.WaitSpawn;
    }

    #region 等待生成
    private int spawnInteractivePointId;
    private Vector3 spawnInteractivePointPos;
    
    public int GetSpawnInteractivePointId()
    {
        return spawnInteractivePointId;
    }
    
    public Vector3 GetSpawnInteractivePointPos()
    {
        if (ScenePointConfigItem == null)
        {
            return new Vector3(0, 0, 0);
        }

        if (spawnInteractivePointPos == null)
        {
            return ScenePointConfigItem.Position;
        }
        return spawnInteractivePointPos;
    }

    private void OnWaitSpawn()
    {
        //等待生成
        int[] spawnInteractivePointIdList = ScenePointConfigItem.BreakInteractiveIds;
        //随机获取
        spawnInteractivePointId = spawnInteractivePointIdList[RandomHelper.GetRandom(0, spawnInteractivePointIdList.Length)];
        float randomX = RandomHelper.GetRandom(ScenePointConfigItem.Position.X - ScenePointConfigItem.RandomRadius, ScenePointConfigItem.Position.X + ScenePointConfigItem.RandomRadius);
        float randomZ = RandomHelper.GetRandom(ScenePointConfigItem.Position.Z - ScenePointConfigItem.RandomRadius, ScenePointConfigItem.Position.Z + ScenePointConfigItem.RandomRadius);
        spawnInteractivePointPos = new Vector3(randomX, ScenePointConfigItem.Position.Y, randomZ);
    }

    #endregion
    
}