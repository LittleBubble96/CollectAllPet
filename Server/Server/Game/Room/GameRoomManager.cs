using System.Collections.Concurrent;
using ShareProtobuf;

public class GameRoomManager : Singleton<GameRoomManager>
{
    private int CurRoomId = 0;
    private ConcurrentDictionary<int, GameRoom> _gameRooms = new ConcurrentDictionary<int, GameRoom>();
      
    public void Init()
    { 
    
    }

    public void Update()
    {
        //foreach (var gameRoom in _gameRooms)
        //{
        //    gameRoom.Value.Update();
        //}
    }

    public CreateRoomResultCallBack CreateRoom(string playerId , string roomName , int maxPlayerCount)
    { 
        GameRoom gameRoom = new GameRoom();
        int roomId = GenerateRoomId();
        if (roomId <= 0)
        {
            return new CreateRoomResultCallBack() { IsSuccess = false, Message = "创建房间失败" };
        }
        gameRoom.Init(playerId,roomId, roomName, maxPlayerCount);
        _gameRooms.TryAdd(gameRoom.RoomId, gameRoom);
        return new CreateRoomResultCallBack() { IsSuccess = true, Message = "创建房间成功", RoomId = roomId };
    }

    public ResultCallBack JoinRoom(string playerId, int roomId)
    {
        if (!_gameRooms.TryGetValue(roomId, out GameRoom gameRoom))
        {
            return new ResultCallBack() { IsSuccess = false, Message = "房间不存在" };
        }
        return gameRoom.AddPlayer(playerId);
    }

    private int GenerateRoomId()
    {
        CurRoomId++;
        int loopCount = 0;
        while (_gameRooms.ContainsKey(CurRoomId))
        {
            CurRoomId++;
            if (CurRoomId > GameConst.RoomIdEnd)
            {
                CurRoomId = GameConst.RoomIdStart;
                loopCount++;
                if (loopCount > 1)
                {
                    Console.WriteLine("房间已满");
                    return -1;
                }
            }
        }
        return CurRoomId;
    }

    public List<SimpleRoomInfo> GetSimpleRoomInfos()
    {
        List<SimpleRoomInfo> simpleRoomInfos = new List<SimpleRoomInfo>();
        foreach (var gameRoom in _gameRooms)
        {
            simpleRoomInfos.Add(gameRoom.Value.GetSimpleRoomInfo());
        }
        return simpleRoomInfos;
    }
}