using ShareProtobuf;

public partial class ClientFactoryRegisterHelper
{
    public static void RegisterMessage()
    {
        ClientFactory.Instance.GetMessageResponseFactory().RegisterType<LoginRequest>(MessageRequestType.PlayerLogin);
        ClientFactory.Instance.GetMessageResponseFactory().RegisterType<HeartbeatResponse>(MessageRequestType.Heratbeat);
        ClientFactory.Instance.GetMessageResponseFactory().RegisterType<RefreshRoomListResponseHandle>(MessageRequestType.RefreshRoomList);
        ClientFactory.Instance.GetMessageResponseFactory().RegisterType<CreateRoomResponseHandle>(MessageRequestType.CreateRoomRequest);
        ClientFactory.Instance.GetMessageResponseFactory().RegisterType<JoinRoomResponseHandle>(MessageRequestType.JoinRoomRequest);
        ClientFactory.Instance.GetMessageResponseFactory().RegisterType<GetRoomDetailResponseHandle>(MessageRequestType.GetRoomDetailRequest);
        ClientFactory.Instance.GetMessageResponseFactory().RegisterType<CreatePlayerResponseHandle>(MessageRequestType.CreateActorRequest);
        ClientFactory.Instance.GetMessageResponseFactory().RegisterType<SyncActorDeltaResponseHandle>(MessageRequestType.SyncActorDetailRequest);
        ClientFactory.Instance.GetMessageResponseFactory().RegisterType<SyncActorAnimationDeltaResponseHandle>(MessageRequestType.SyncActorAnimationDeltaRequest);
    }
}