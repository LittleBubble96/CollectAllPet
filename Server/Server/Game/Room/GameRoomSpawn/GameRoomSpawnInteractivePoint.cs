
using ShareProtobuf;
using ShareProtobuf.ShareData;

public enum SpawnInteractiveState
{
    None = 0,
    WaitSpawn = 1,
    Spawn = 2,
    WaitTarget = 3,
    Targeting = 4,
    Destroy = 5,
}
public class GameRoomSpawnInteractivePoint
{
    public ScenePointConfigItem ScenePointConfigItem { get; set; }
    
    private float _stateTimeCount = 0;

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
                    SpawnInteractiveState = SpawnInteractiveState.WaitTarget;
                    break;
                case SpawnInteractiveState.WaitTarget:
                    OnWaitTarget();
                    break;
                case SpawnInteractiveState.Destroy:
                    _stateTimeCount = ScenePointConfigItem.ReSpawnTime;
                    break;
            }
        }
    }
    
    public void Init( ScenePointConfigItem scenePointConfigItem)
    {
        ScenePointConfigItem = scenePointConfigItem;
        SpawnInteractiveState = SpawnInteractiveState.WaitSpawn;
    }
    
    public void OnUpdate(double deltaTime)
    {
        //更新
        if (SpawnInteractiveState == SpawnInteractiveState.Destroy)
        {
            _stateTimeCount -= (float)deltaTime;
            if (_stateTimeCount <= 0)
            {
                SpawnInteractiveState = SpawnInteractiveState.WaitSpawn;
            }
        }
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

    #region 生成结束

    private int spawnActorId;
    
    public void OnSpawnEnd(int spawnActorId)
    {
        this.spawnActorId = spawnActorId;
        SpawnInteractiveState = SpawnInteractiveState.Spawn;
    }
    
    public int GetSpawnActorId()
    {
        return spawnActorId;
    }

    #endregion

    #region 销毁

    public void OnDestroy(RoomActor actor)
    {
        //销毁
        SpawnInteractiveState = SpawnInteractiveState.Destroy;
    }

    #endregion

    #region 目标
    
    protected void OnWaitTarget()
    {
        //进入等待 目标
    }
    
    public void OnTargeting(int targetActorId)
    {
        //进入目标
        spawnInteractiveState = SpawnInteractiveState.Targeting;
    }

    #endregion
}