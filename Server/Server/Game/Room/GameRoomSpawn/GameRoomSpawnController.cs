
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
        if (_lastUpdateTime >= _updateInterval)
        {
            // 收集所有的WaitSpawn
            CollectWaitSpawn();
            _lastUpdateTime = 0;
        }
    }
    
    private void CollectWaitSpawn()
    {
        WaitSpawnPoints.Clear();
        // Collect all WaitSpawn
        foreach (var point in InteractivePoints)
        {
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
                CreateActorResultCallBack callBack = room.RoomWorld.AddActor("SceneInstance", EActorRoleType.BreakInteractive, point.GetSpawnInteractivePointId(), point.GetSpawnInteractivePointPos(), point.ScenePointConfigItem.Rotation);
                point.OnSpawnEnd(callBack.ActorId);
                actorIds.Add(callBack.ActorId);
            }
            //发送创建事件
            List<GameActorInfo> actorInfos = room.RoomWorld.GetActors(actorIds);
            CreateRoomActorToClientRequest createRoomActorToClientRequest = new CreateRoomActorToClientRequest
            {
                RoomId = room.RoomId,
                Actors = actorInfos,
            };

            room.SendMessageToAllClientNoTask(MessageRequestType.CreateActorRequestToClient, createRoomActorToClientRequest);

        }
       
    }
    
}