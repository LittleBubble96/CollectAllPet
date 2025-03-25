using ShareProtobuf;
using System.Numerics;

public class GameRoom
{
    public string HostPlayerId { get; private set; }
    public int RoomId { get; private set; }

    public string RoomName { get; private set; }

    public int MaxPlayerCount { get; private set; }

    public Dictionary<string, RoomPlayer> Players = new Dictionary<string, RoomPlayer>();
    
    public RoomWorld RoomWorld {get; private set;}

    private object _lock = new object();

    public void Init(string playerId,string clientIPAndPort , int roomId , string roomName , int maxPlayerCount)
    {
        HostPlayerId = playerId;
        RoomId = roomId;
        RoomName = roomName;
        MaxPlayerCount = maxPlayerCount;
        RoomWorld = new RoomWorld();
        RoomWorld.Init(roomId);
        AddPlayer(playerId,clientIPAndPort);
    }

    public ResultCallBack AddPlayer(string playerId,string clientIPAndPort)
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
            roomPlayer.Init(playerId ,clientIPAndPort, RoomId, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
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
    
    public RoomActor GetRoomActorByPlayerId(string playerId)
    {
        return RoomWorld.GetRoomActorByPlayerId(playerId);
    }

    public void SyncActors(string playerId, List<DeltaActorSyncData> actors)
    {
        RoomWorld.SyncActors(playerId, actors);
    }
}