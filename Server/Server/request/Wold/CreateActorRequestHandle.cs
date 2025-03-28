

using ShareProtobuf;

public class CreateActorRequestHandle : MessageRquestBase
{
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        CreatePlayerActorRequest createActorRequest = await GetClientHandle().ReceiveMessage<CreatePlayerActorRequest>(messageBuffer);
        Console.WriteLine("CreatePlayerActorRequest PlayerId: " + createActorRequest.PlayerId + " RoomId: " + createActorRequest.RoomId);

        GameRoom gameRoom = GameRoomManager.Instance.GetGameRoom(createActorRequest.RoomId);
        PlayerProxy playerProxy = PlayerManager.Instance.GetPlayer(createActorRequest.PlayerId);
        if (gameRoom == null || playerProxy == null)
        {
            CreatePlayerActorResponse createActorResponse = new CreatePlayerActorResponse
            {
                IsSuccess = false,
                Message = "Room not exist",
            };
            await GetClientHandle().SendMessage(MessageRequestType.CreateActorResponse, createActorResponse);
            return;
        }

        List<int> actorIds = new List<int>();
        CreateActorResultCallBack actorIdResult = gameRoom.RoomWorld.AddActor(createActorRequest.PlayerId, EActorRoleType.Player, playerProxy.PlayerData.playerConfigId, createActorRequest.SpawnPos, createActorRequest.SpawnRot);

        if (!actorIdResult.IsSuccess)
        {
            CreatePlayerActorResponse createActorResponse = new CreatePlayerActorResponse
            {
                IsSuccess = false,
                Message = actorIdResult.Message,
            };
            await GetClientHandle().SendMessage(MessageRequestType.CreateActorResponse, createActorResponse);
            return;
        }
        actorIds.Add(actorIdResult.ActorId);
        //添加宠物actor
        foreach (var petData in playerProxy.PlayerData.playerPetDatas)
        {
            CreateActorResultCallBack petaActorIdResult = gameRoom.RoomWorld.AddActor(createActorRequest.PlayerId, EActorRoleType.Monster, petData.petConfigId, createActorRequest.SpawnPos, createActorRequest.SpawnRot);
            if (!petaActorIdResult.IsSuccess)
            {
                CreatePlayerActorResponse createActorResponse = new CreatePlayerActorResponse
                {
                    IsSuccess = false,
                    Message = petaActorIdResult.Message,
                };
                await GetClientHandle().SendMessage(MessageRequestType.CreateActorResponse, createActorResponse);
                return;
            }
            actorIds.Add(actorIdResult.ActorId);
        }
        CreatePlayerActorResponse createActorResponseSuc = new CreatePlayerActorResponse
        {
            RefActorId = gameRoom.GetRoomActorByPlayerId(createActorRequest.PlayerId).ActorId,
            IsSuccess = actorIdResult.IsSuccess,
            Message = actorIdResult.Message,
        };
        await GetClientHandle().SendMessage(MessageRequestType.CreateActorResponse, createActorResponseSuc);
        //发送给所有客户端
        List<GameActorInfo> gameActorInfos = new List<GameActorInfo>();
        foreach (var actorId in actorIds)
        {
            gameActorInfos.Add(gameRoom.RoomWorld.GetActorInfo(actorId));
        }
        CreateRoomActorToClientRequest createRoomActorToClientRequest = new CreateRoomActorToClientRequest
        {
            RoomId = createActorRequest.RoomId,
            Actors = gameActorInfos,
        };

        List<ClientHandle> clientHandles = gameRoom.GetClientHandles();
        foreach (var clientHandle in clientHandles)
        {
            if (clientHandle == null)
            {
                continue;
            }
            await clientHandle.SendMessage(MessageRequestType.CreateActorRequestToClient, createRoomActorToClientRequest);
        }
    }
}