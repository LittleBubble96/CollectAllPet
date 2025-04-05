using ShareProtobuf;

public partial class ClientFactoryRegisterHelper
{
    public static void RegisterRequestMessage()
    {
        ClientFactory.Instance.GetMessageRequestFactory().RegisterType<CreateActorRequestHandle>(MessageRequestType.CreateActorRequestToClient);
        ClientFactory.Instance.GetMessageRequestFactory().RegisterType<SyncActorAnimationDeltaRequestHandle>(MessageRequestType.SyncActorAnimationDetailRequestToClient);
        ClientFactory.Instance.GetMessageRequestFactory().RegisterType<SyncActorAttributeRequestHandle>(MessageRequestType.SyncActorAttributeRequestToClient);
        ClientFactory.Instance.GetMessageRequestFactory().RegisterType<PlayEffectRequestHandle>(MessageRequestType.PlayEffectRequestToClient);
    }
}