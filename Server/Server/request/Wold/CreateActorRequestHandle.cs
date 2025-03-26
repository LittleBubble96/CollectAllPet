
using ShareProtobuf;
using ShareProtobuf.ShareData;

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
        PlayerConfigItem playerConfigItem = PlayerConfig.GetPlayerConfigItem(playerProxy.PlayerData.playerConfigId);
        CreateActorResultCallBack actorIdResult = gameRoom.RoomWorld.AddActor(createActorRequest.PlayerId,EActorRoleType.Player,playerConfigItem.Prefab,createActorRequest.SpawnPos, createActorRequest.SpawnRot);

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
        CreatePlayerActorResponse createActorResponseSuc = new CreatePlayerActorResponse
        {
            RoomDetailInfo = gameRoom.GetRoomDetailInfo(),
            RefActorId = gameRoom.GetRoomActorByPlayerId(createActorRequest.PlayerId).ActorId,
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