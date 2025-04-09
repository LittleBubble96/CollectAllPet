
using System.Collections.Concurrent;
using ShareProtobuf;
using ShareProtobuf.ShareData;

public class GameRoomSpawnController
{
    public List<GameRoomSpawnInteractivePoint> InteractivePoints { get; private set; }
    public Queue<GameRoomSpawnInteractivePoint > WaitSpawnPoints { get; private set; }
    
    private double _updateInterval = 0.1f;
    private double _lastUpdateTime = 0;
    private GameRoom room = null;
    
    private Task spawnScenePointTask = null;

    
    public void Init(GameRoom inRoom)
    {
        room = inRoom;
        InteractivePoints = new List<GameRoomSpawnInteractivePoint>();
        WaitSpawnPoints = new Queue<GameRoomSpawnInteractivePoint>();
        InitInteractivePoints();
    }
    
    private void InitInteractivePoints()
    {
        // Initialize interactive points
        foreach (var cfg in ScenePointConfig.ConfigDict)
        {
            var point = new GameRoomSpawnInteractivePoint();
            point.Init(cfg.Value);
            InteractivePoints.Add(point);
        }
        CollectWaitSpawn();
    }
    
    public void DoUpdate(double deltaTime)
    {
        _lastUpdateTime += deltaTime;
        if (spawnScenePointTask != null && !spawnScenePointTask.IsCompleted)
        {
            return;
        }
        if (_lastUpdateTime >= _updateInterval)
        {
            // 收集所有的WaitSpawn
            CollectWaitSpawn();
            _lastUpdateTime = 0;
        }
        for (int i = InteractivePoints.Count - 1; i >= 0; i--)
        {
            var point = InteractivePoints[i];
            point.OnUpdate(deltaTime);
        }
    }
    
    private void CollectWaitSpawn()
    {
        WaitSpawnPoints.Clear();
        // Collect all WaitSpawn
        for (int i = InteractivePoints.Count -1; i >=0; i--)
        {
            var point = InteractivePoints[i];
            if (point.SpawnInteractiveState == SpawnInteractiveState.WaitSpawn)
            {
                // Do something with the wait spawn
                WaitSpawnPoints.Enqueue(point);
            }
        }
        if (WaitSpawnPoints.Count > 0)
        {
            List<int> actorIds = new List<int>();

            // Do something with the collected wait spawn points
            while (WaitSpawnPoints.Count > 0)
            {
                var point = WaitSpawnPoints.Dequeue();
                // Do something with the point
                //发送创建事件
                CreateActorResultCallBack callBack = room.RoomWorld.AddActor(-1, EActorRoleType.BreakInteractive, point.GetSpawnInteractivePointId(), point.GetSpawnInteractivePointPos(), point.ScenePointConfigItem.Rotation);
                RoomActor roomActor = room.RoomWorld.GetActor(callBack.ActorId);
                if (roomActor == null)
                {
                    continue;
                }
                point.OnSpawnEnd(callBack.ActorId);
                roomActor.OnDestroyAction += point.OnDestroy;
                actorIds.Add(callBack.ActorId);
            }
            //发送创建事件
            List<GameActorInfo> actorInfos = room.RoomWorld.GetActors(actorIds);
            CreateRoomActorToClientRequest createRoomActorToClientRequest = new CreateRoomActorToClientRequest
            {
                RoomId = room.RoomId,
                Actors = actorInfos,
            };

            spawnScenePointTask = room.SendMessageToAllClient(MessageRequestType.CreateActorRequestToClient, createRoomActorToClientRequest);

        }
       
    }

    public int FindWaitTargetScenePoint(int petActorId , int lastInteractiveId)
    {
        PetActor actor = room.RoomWorld.GetPet(petActorId);
        if (actor == null)
        {
            return -1;
        }
        BreakInteractiveActor breakInteractiveActor = room.RoomWorld.GetBreakInteractive(lastInteractiveId);
        if (breakInteractiveActor != null)
        {
            //检查与宠物的取消攻击距离 是否小于等于
            //则返回
            float distance = Vector3.Distance(actor.Pos, breakInteractiveActor.Pos);
            if (distance <= actor.GetCancelAttackRange())
            {
                return lastInteractiveId;
            }
        }
        //重新找一个最近的
        float minDistance = float.MaxValue;
        GameRoomSpawnInteractivePoint targetPoint = null;
        foreach (var point in InteractivePoints)
        {
            if (point.SpawnInteractiveState == SpawnInteractiveState.WaitTarget)
            {
                float distance = Vector3.Distance(actor.Pos, point.GetSpawnInteractivePointPos());
                if (distance < minDistance)
                {
                    minDistance = distance;
                    targetPoint = point;
                }
            }
        }
        if (targetPoint != null)
        {
            targetPoint.OnTargeting(petActorId);
            return targetPoint.GetSpawnActorId();
        }
        //没有找到
        return -1;
    }

}