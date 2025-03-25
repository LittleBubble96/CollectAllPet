using ShareProtobuf;

public class RoomManager : Singleton<RoomManager>
{
    private int enterRoomId = -1;
    private ERoomState roomState = ERoomState.None;
    private RoomDetailInfo roomDetailInfo;
    private int refActorId;

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
                    //发送消息
                    OnWaitRoom();
                    break;
                case ERoomState.Loading:
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
    
    // Update Room Detail refActorId is Host
    public void UpdateDetailRoom(RoomDetailInfo roomDetail,int inRefActorId)
    {
        this.roomDetailInfo = roomDetail;
        this.refActorId = inRefActorId;
    }
    
    public RoomDetailInfo GetRoomDetailInfo()
    {
        return roomDetailInfo;
    }
    
    public int GetRefActorId()
    {
        return refActorId;
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
        ClientRequestFunc.GetRoomDetailRequest(enterRoomId,CharacterManager.Instance.PlayerInfo.PlayerId);
    }
    
    public void SpawnRoomWorld()
    {
        // Spawn Room World
    }

    #endregion
}