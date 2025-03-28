
namespace ShareProtobuf
{
    public enum MessageRequestType
    {
        None,
        PlayerLogin,
        PlayerLoginResponse,
        Heratbeat,
        HeartbeatAck,

        //房间消息
        CreateRoomResponse,
        CreateRoomRequest,

        RefreshRoomList,
        RefreshRoomListResponse,

        JoinRoomRequest,
        JoinRoomResponse,
        
        GetRoomDetailRequest,
        GetRoomDetailResponse,
        
        CreateActorRequest,
        CreateActorResponse,
        
        SyncActorDetailRequest,
        SyncActorDetailResponse,
        
        SyncActorAnimationDeltaRequest,
        SyncActorAnimationDeltaResponse,
        
        //Server to Client
        CreateActorRequestToClient,
        SyncActorAnimationDetailRequestToClient,
    }


}