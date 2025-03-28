using System.Threading.Tasks;
using ShareProtobuf;

public class CreateActorRequestHandle : MessageRequestBase
{
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        CreateRoomActorToClientRequest response = await GameManager.GetNetworkManager().ReceiveMessage<CreateRoomActorToClientRequest>(messageBuffer);
        for (int i = 0; i < response.Actors.Count; i++)
        {
            RoomManager.Instance.CreateRoomActor(response.Actors[i]);
        }
    }
}