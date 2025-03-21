public class RoomManager : Singleton<RoomManager>
{
    private int enterRoomId = -1;
    
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
}