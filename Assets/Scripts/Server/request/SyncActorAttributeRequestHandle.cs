using System.Threading.Tasks;
using ShareProtobuf;

public class SyncActorAttributeRequestHandle : MessageRequestBase
{
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        SyncActorAttributeToClientRequest response = await GameManager.GetNetworkManager().ReceiveMessage<SyncActorAttributeToClientRequest>(messageBuffer);
        RoomManager.Instance.SyncServerActorPropertiesInfo(response.ActorIds,response.UpdateAttributes);
    }
}