using ShareProtobuf;
using System.Numerics;

public class GameRoom
{
    public string HostPlayerId { get; private set; }
    public int RoomId { get; private set; }

    public string RoomName { get; private set; }

    public int MaxPlayerCount { get; private set; }

    public Dictionary<string, RoomPlayer> Players = new Dictionary<string, RoomPlayer>();

    private object _lock = new object();

    public void Init(string playerId , int roomId , string roomName , int maxPlayerCount)
    {
        HostPlayerId = playerId;
        RoomId = roomId;
        RoomName = roomName;
        MaxPlayerCount = maxPlayerCount;
        AddPlayer(playerId);
    }

    public ResultCallBack AddPlayer(string playerId)
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
            roomPlayer.Init(playerId, RoomId, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
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
}