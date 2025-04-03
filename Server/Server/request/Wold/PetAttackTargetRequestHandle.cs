

using ShareProtobuf;

public class PetAttackTargetRequestHandle : MessageRquestBase
{
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        PetAttackTargetActorRequest request = await GetClientHandle().ReceiveMessage<PetAttackTargetActorRequest>(messageBuffer);
        Console.WriteLine("PetAttackTargetActorRequest RoomName: {0}", request.RoomId);
        GameRoom room = GameRoomManager.Instance.GetGameRoom(request.RoomId);
        if (room == null)
        {
            PetAttackTargetActorResponse response = new PetAttackTargetActorResponse
            {
                IsSuccess = false,
                Message = "Room not found",
            };
            await GetClientHandle().SendMessage(MessageRequestType.PetAttackTargetResponse, response);
            return;
        }
        // Actor actor = room.GetActor(request.ActorId);
    }
}