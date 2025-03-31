

using ShareProtobuf;

public class SyncActorAnimationDeltaRequestHandle : MessageRquestBase
{
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        SyncActorAnimationToServerRequest deltaActorSync = await GetClientHandle().ReceiveMessage<SyncActorAnimationToServerRequest>(messageBuffer);
        Console.WriteLine("SyncActorAnimationToServerRequest , RoomId: {0}", deltaActorSync.RoomId);
        GameRoom gameRoom = GameRoomManager.Instance.GetGameRoom(deltaActorSync.RoomId);
        if (gameRoom == null)
        {
            SyncActorAnimationToServerResponse responseFa = new SyncActorAnimationToServerResponse
            {
                IsSuccess = false,
                Message = "Room not exist",
            };
            await GetClientHandle().SendMessage(MessageRequestType.SyncActorAnimationDeltaResponse, responseFa);
            return;
        }
        SyncActorAnimationToServerResponse response = new SyncActorAnimationToServerResponse
        {
            IsSuccess = true,
        };
        await GetClientHandle().SendMessage(MessageRequestType.SyncActorAnimationDeltaResponse, response);
        SyncActorAnimationToClientRequest deltaActorSyncResponseSuc = new SyncActorAnimationToClientRequest
        {
            Actors = deltaActorSync.Actors,
        };

        await gameRoom.SendMessageToAllClient(MessageRequestType.SyncActorAnimationDetailRequestToClient, deltaActorSyncResponseSuc);
    }
}