using ShareProtobuf;

public class RequestMessageFactoryHelper
{
    public static void Register()
    {
        ServerFactory.Instance.GetMessageRequestFactory().RegisterType<PlayerLoginRequest>(MessageRequestType.PlayerLogin);
        ServerFactory.Instance.GetMessageRequestFactory().RegisterType<HeartbeatRequest>(MessageRequestType.Heratbeat);
        ServerFactory.Instance.GetMessageRequestFactory().RegisterType<RefreshRoomListRequestHandle>(MessageRequestType.RefreshRoomList);
        ServerFactory.Instance.GetMessageRequestFactory().RegisterType<CreateRoomRequestHandle>(MessageRequestType.CreateRoomRequest);
        ServerFactory.Instance.GetMessageRequestFactory().RegisterType<JoinRoomRequestHandle>(MessageRequestType.JoinRoomRequest);
        ServerFactory.Instance.GetMessageRequestFactory().RegisterType<CreateActorRequestHandle>(MessageRequestType.CreateActorRequest);
        ServerFactory.Instance.GetMessageRequestFactory().RegisterType<SyncActorDeltaRequestHandle>(MessageRequestType.SyncActorDetailRequest);
        ServerFactory.Instance.GetMessageRequestFactory().RegisterType<GetRoomDetailRequestHandle>(MessageRequestType.GetRoomDetailRequest);
        ServerFactory.Instance.GetMessageRequestFactory().RegisterType<SyncActorAnimationDeltaRequestHandle>(MessageRequestType.SyncActorAnimationDeltaRequest);
        ServerFactory.Instance.GetMessageRequestFactory().RegisterType<FindPetTargetRequestHandle>(MessageRequestType.FindPetTargetRequest);
    }
}