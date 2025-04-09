using System.Collections.Concurrent;
using ShareProtobuf;

public class GameRoom
{
    public int HostPlayerId { get; private set; }
    public int RoomId { get; private set; }

    public string RoomName { get; private set; }

    public int MaxPlayerCount { get; private set; }

    public Dictionary<int, RoomPlayer> Players = new Dictionary<int, RoomPlayer>();
    
    public RoomWorld RoomWorld {get; private set;}
    public GameRoomSpawnController SpawnController { get; private set; }
    
    public GameRoomEffectController EffectController { get; private set; }

    private object _lock = new object();
    
    private Task destroyActorTask = null;
    private Task updateActorAttrTask = null;

    public void Init(int playerId,string clientIPAndPort , int roomId , string roomName , int maxPlayerCount)
    {
        HostPlayerId = playerId;
        RoomId = roomId;
        RoomName = roomName;
        MaxPlayerCount = maxPlayerCount;
        RoomWorld = new RoomWorld();
        RoomWorld.Init(roomId);
        SpawnController = new GameRoomSpawnController();
        SpawnController.Init(this);
        EffectController = new GameRoomEffectController(this);
        AddPlayer(playerId,clientIPAndPort);
    }
    
    public void Update(double deltaMSTime)
    {
        if (SpawnController != null)
        {
            SpawnController.DoUpdate(deltaMSTime);
        }
        //各个客户端更新 修改后的属性
        if (RoomWorld != null && RoomWorld.RoomWorldAttIsDirty())
        {
            if (updateActorAttrTask == null || updateActorAttrTask.IsCompleted)
            {
                UpdateActorAttrDict();

            }
        }
        //各个客户端更新 需要播放得特效
        if (EffectController != null)
        {
            EffectController.DoFixedUpdate();
        }
        
        //销毁Actor
        if (RoomWorld != null && RoomWorld.WaitDestroyActors.Count > 0)
        {
            if (destroyActorTask == null || destroyActorTask.IsCompleted)
            {
                destroyActorTask = DestroyActorAttrDict();
            }
        }
    }
    
    public GameRoomEffectController GetEffectController()
    {
        return EffectController;
    }

    public ResultCallBack AddPlayer(int playerId,string clientIPAndPort)
    {
        lock (_lock)
        {
            if (Players.ContainsKey(playerId))
            {
                return new ResultCallBack() { IsSuccess = false, Message = "玩家已在房间中" };
            }
            if (Players.Count >= MaxPlayerCount)
            {
                return new ResultCallBack() { IsSuccess = false, Message = "房间已满" };
            }
            RoomPlayer roomPlayer = new RoomPlayer();
            roomPlayer.Init(playerId ,clientIPAndPort, RoomId, GameConst.ZeroVector3, GameConst.ZeroVector3);
            Players.Add(playerId, roomPlayer);
            return new ResultCallBack() { IsSuccess = true, Message = "加入房间成功" };
        }
    }

    public SimpleRoomInfo GetSimpleRoomInfo()
    {
        SimpleRoomInfo simpleRoomInfo = new SimpleRoomInfo();
        simpleRoomInfo.RoomId = RoomId;
        simpleRoomInfo.RoomName = RoomName;
        simpleRoomInfo.PlayerCount = Players.Count;
        simpleRoomInfo.MaxPlayerCount = MaxPlayerCount;
        simpleRoomInfo.Players = new List<SimplePlayerInfo>();
        foreach (var player in Players)
        {
            simpleRoomInfo.Players.Add(player.Value.GetSimplePlayerInfo());
        }
        return simpleRoomInfo;
    }
    
    //获取所有的ClientHandle
    public List<ClientHandle> GetClientHandles()
    {
        List<ClientHandle> clientHandles = new List<ClientHandle>();
        foreach (var player in Players)
        {
            clientHandles.Add(GameServe.Instance.GetClientHandle(player.Value.ClientIPAndPort));
        }
        return clientHandles;
    }
    
    public async Task SendMessageToAllClient(MessageRequestType type, object message)
    {
        List<ClientHandle> clientHandles = GetClientHandles();
        foreach (var clientHandle in clientHandles)
        {
            if (clientHandle == null)
            {
                continue;
            }
            await clientHandle.SendMessage(type, message);
        }
    }
    
    public async void SendMessageToAllClientNoTask(MessageRequestType type, object message)
    {
        List<ClientHandle> clientHandles = GetClientHandles();
        foreach (var clientHandle in clientHandles)
        {
            if (clientHandle == null)
            {
                continue;
            }
            await clientHandle.SendMessage(type, message);
        }
    }
    
    
    //获取房间详细信息
    public RoomDetailInfo GetRoomDetailInfo()
    {
        RoomDetailInfo roomDetailInfo = new RoomDetailInfo();
        roomDetailInfo.RoomId = RoomId;
        roomDetailInfo.RoomName = RoomName;
        roomDetailInfo.PlayerCount = Players.Count;
        roomDetailInfo.MaxPlayerCount = MaxPlayerCount;
        roomDetailInfo.WorldActors = RoomWorld.GetActors();
        return roomDetailInfo;
    }
    
    public RoomActor GetRoomActorByPlayerId(int playerId)
    {
        return RoomWorld.GetRoomActorByPlayerId(playerId);
    }

    public void SyncActors(int playerId, List<DeltaActorSyncData> actors)
    {
        if (actors == null)
        {
            return;
        }
        RoomWorld.SyncActors(playerId, actors);
    }

    #region Actor属性更新

    protected void UpdateActorAttrDict()
    {
        SyncActorAttributeToClientRequest syncActorAttributeToClientRequest = new SyncActorAttributeToClientRequest();
        syncActorAttributeToClientRequest.ActorIds = new List<int>();
        syncActorAttributeToClientRequest.UpdateAttributes = new List<string>();
        RoomWorld.OptionRoomActor((actor) =>
        {
            if (actor.RoomWorldAttIsDirty())
            {
                syncActorAttributeToClientRequest.ActorIds.Add(actor.ActorId);
                syncActorAttributeToClientRequest.UpdateAttributes.Add(actor.GetDirtyAttributeJson());
                actor.ResetDirtyAttribute();
            }
        });
        
        
        SendMessageToAllClientNoTask(MessageRequestType.SyncActorAttributeRequestToClient,syncActorAttributeToClientRequest);
    }
    
    protected async Task DestroyActorAttrDict()
    {
        SyncDestroyActorToClientRequest syncDestroyActorToClientRequest = new SyncDestroyActorToClientRequest();
        syncDestroyActorToClientRequest.Actors = new List<SyncDestroyActorToClientData>();
        foreach (var actor in RoomWorld.WaitDestroyActors)
        {
            syncDestroyActorToClientRequest.Actors.Add(new SyncDestroyActorToClientData()
            {
                ActorId = actor.Value.ActorId,
            });
        }
        RoomWorld.ClearWaitDestroyActor();
        await SendMessageToAllClient(MessageRequestType.DestroyActorRequestToClient,syncDestroyActorToClientRequest);
    }
    
    #endregion

    #region 宠物逻辑

    

    #endregion
}