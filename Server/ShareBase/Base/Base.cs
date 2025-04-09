
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
        
        //查找宠物目标
        FindPetTargetRequest,
        FindPetTargetResponse,
        
        //宠物攻击目标
        PetAttackTargetRequest,
        PetAttackTargetResponse,
        
        //添加宠物
        AddPetRequest,
        AddPetResponse,

        #region Server to Client
        
        CreateActorRequestToClient,
        SyncActorAnimationDetailRequestToClient,
        SyncActorAttributeRequestToClient,
        //Actor销毁逻辑
        DestroyActorRequestToClient,
        //播放特效
        PlayEffectRequestToClient,
        #endregion
        
    }


}