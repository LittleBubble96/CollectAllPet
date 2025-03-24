public class RoomManager : Singleton<RoomManager>
{
    private int enterRoomId = -1;
    private ERoomState roomState = ERoomState.None;

    public ERoomState RoomState
    {
        get { return roomState; }
        set
        {
            if (roomState == value) return;
            roomState = value;
            switch (roomState)
            {
                case ERoomState.None:
                    break;
                case ERoomState.Waiting:
                    break;
                case ERoomState.Playing:
                    break;
                case ERoomState.End:
                    break;
            }
        }
    }

    public void Init()
    {
        // Init RoomManager
    }
    
    public void EnterRoom(int roomId)
    {
        // Enter Room
        enterRoomId = roomId;
    }
    
    public void CreateRoom(string roomName ,int maxPlayerCount)
    {
        // Create Room
        ClientRequestFunc.SendCreateRoomRequest(roomName,maxPlayerCount);
    }
    
    public void JoinRoom(int roomId)
    {
        // Join Room
        ClientRequestFunc.SendJoinRoomRequest(roomId);
    }
    
    public void RefreshRoomList()
    {
        // Refresh Room List
        ClientRequestFunc.RefreshRoomListRequest();
    }

    #region 房间世界
    public void OnWaitRoom()
    {
        // Wait Room
        //开启协程
    }
    
    public void SpawnRoomWorld()
    {
        // Spawn Room World
    }

    #endregion
}