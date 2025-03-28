
using System.Threading.Tasks;
using ShareProtobuf;

public class SyncActorAnimationDeltaRequestHandle : MessageRequestBase
{
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        SyncActorAnimationToClientRequest response = await GameManager.GetNetworkManager().ReceiveMessage<SyncActorAnimationToClientRequest>(messageBuffer);
        RoomManager.Instance.SyncServerActorAnimationInfo(response.Actors);
        GameManager.GetGameSyncActorManager().ReceiveSyncActorAnimationResponse();
    }
}