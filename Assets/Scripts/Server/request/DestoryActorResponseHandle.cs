

using System.Threading.Tasks;
using ShareProtobuf;

public class DestoryActorResponseHandle : MessageRequestBase
{
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        SyncDestroyActorToClientRequest response = await GameManager.GetNetworkManager().ReceiveMessage<SyncDestroyActorToClientRequest>(messageBuffer);
        for (int i = 0; i < response.Actors.Count; i++)
        {
            RoomManager.Instance.DestroyActor(response.Actors[i].ActorId);
        }
    }
}