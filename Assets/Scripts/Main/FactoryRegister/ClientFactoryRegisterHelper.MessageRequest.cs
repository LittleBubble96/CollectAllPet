using ShareProtobuf;

public partial class ClientFactoryRegisterHelper
{
    public static void RegisterRequestMessage()
    {
        ClientFactory.Instance.GetMessageRequestFactory().RegisterType<CreateActorRequestHandle>(MessageRequestType.CreateActorRequestToClient);
        ClientFactory.Instance.GetMessageRequestFactory().RegisterType<SyncActorAnimationDeltaRequestHandle>(MessageRequestType.SyncActorAnimationDetailRequestToClient);
    }
}