using ShareProtobuf;

public class RequestMessageFactoryHelper
{
    public static void Register()
    {
        ServerFactory.Instance.GetMessageRquestFactory().RegisterType<PlayerLoginRequest>(MessageRequestType.PlayerLogin);
        ServerFactory.Instance.GetMessageRquestFactory().RegisterType<HeartbeatRequest>(MessageRequestType.Heratbeat);
        ServerFactory.Instance.GetMessageRquestFactory().RegisterType<RefreshRoomListRequestHandle>(MessageRequestType.RefreshRoomList);
        ServerFactory.Instance.GetMessageRquestFactory().RegisterType<CreateRoomRequestHandle>(MessageRequestType.CreateRoomRequest);
        ServerFactory.Instance.GetMessageRquestFactory().RegisterType<JoinRoomRequestHandle>(MessageRequestType.JoinRoomRequest);
        ServerFactory.Instance.GetMessageRquestFactory().RegisterType<CreateActorRequestHandle>(MessageRequestType.CreateActorRequest);
        ServerFactory.Instance.GetMessageRquestFactory().RegisterType<SyncActorDeltaRequestHandle>(MessageRequestType.SyncActorDetailRequest);
        ServerFactory.Instance.GetMessageRquestFactory().RegisterType<GetRoomDetailRequestHandle>(MessageRequestType.GetRoomDetailRequest);
        ServerFactory.Instance.GetMessageRquestFactory().RegisterType<SyncActorAnimationDeltaRequestHandle>(MessageRequestType.SyncActorAnimationDeltaRequest);
    }
}