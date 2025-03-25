
using ShareProtobuf;

public class CreateActorRequestHandle : MessageRquestBase
{
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        CreateActorRequest createActorRequest = await GetClientHandle().ReceiveMessage<CreateActorRequest>(messageBuffer);
        Console.WriteLine("CreateActorRequest PlayerId: " + createActorRequest.PlayerId + " RoomId: " + createActorRequest.RoomId);
        
        GameRoom gameRoom = GameRoomManager.Instance.GetGameRoom(createActorRequest.RoomId);
        if (gameRoom == null)
        {
            CreateActorResponse createActorResponse = new CreateActorResponse
            {
                IsSuccess = false,
                Message = "Room not exist",
            };
            await GetClientHandle().SendMessage(MessageRequestType.CreateActorResponse, createActorResponse);
            return;
        }
        CreateActorResultCallBack actorIdResult = gameRoom.RoomWorld.AddActor(createActorRequest.PlayerId,createActorRequest.ResName,createActorRequest.SpawnPos, createActorRequest.SpawnRot);

        if (!actorIdResult.IsSuccess)
        {
            CreateActorResponse createActorResponse = new CreateActorResponse
            {
                IsSuccess = false,
                Message = actorIdResult.Message,
            };
            await GetClientHandle().SendMessage(MessageRequestType.CreateActorResponse, createActorResponse);
            return;
        }
        CreateActorResponse createActorResponseSuc = new CreateActorResponse
        {
            OwnerPlayerId= createActorRequest.PlayerId,
            ActorId = actorIdResult.ActorId,
            ResName = createActorRequest.ResName,
            IsSuccess = actorIdResult.IsSuccess,
            Message = actorIdResult.Message,
        };
        List<ClientHandle> clientHandles = gameRoom.GetClientHandles();
        foreach (var clientHandle in clientHandles)
        {
            await clientHandle.SendMessage(MessageRequestType.CreateActorResponse, createActorResponseSuc);
        }
    }
}