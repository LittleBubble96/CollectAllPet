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
    }
}