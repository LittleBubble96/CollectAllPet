

using ShareProtobuf;

public class FindPetTargetRequestHandle : MessageRquestBase
{
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        FindPetTargetActorIdRequest request = await GetClientHandle().ReceiveMessage<FindPetTargetActorIdRequest>(messageBuffer);
        Console.WriteLine("FindPetTargetRequestHandle");
        GameRoom room = GameRoomManager.Instance.GetGameRoom(request.RoomId);
        if (room == null)
        {
            FindPetTargetActorIdResponse response = new FindPetTargetActorIdResponse
            {
                IsSuccess = false,
                PetActorId = request.PetActorId,
                TargetActorId = -1,
            };
            await GetClientHandle().SendMessage(MessageRequestType.FindPetTargetResponse, response);
            return;
        }

        int targetId = room.SpawnController.FindWaitTargetScenePoint(request.PetActorId, request.LastTargetActorId);
        FindPetTargetActorIdResponse findPetTargetActorIdResponse = new FindPetTargetActorIdResponse
        {
            IsSuccess = true,
            TargetActorId = targetId,
            PetActorId = request.PetActorId,
        };
        await GetClientHandle().SendMessage(MessageRequestType.FindPetTargetResponse, findPetTargetActorIdResponse);
    }
}