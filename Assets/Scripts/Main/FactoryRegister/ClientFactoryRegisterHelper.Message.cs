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
    }
}