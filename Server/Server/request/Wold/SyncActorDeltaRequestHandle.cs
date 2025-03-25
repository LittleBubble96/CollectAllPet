
using ShareProtobuf;

public class SyncActorDeltaRequestHandle : MessageRquestBase
{
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        DeltaActorSyncRequest deltaActorSync = await GetClientHandle().ReceiveMessage<DeltaActorSyncRequest>(messageBuffer);
        Console.WriteLine("DeltaActorSyncRequest PlayerId: {0} , RoomId: {1}", deltaActorSync.PlayerId, deltaActorSync.RoomId);
        GameRoom gameRoom = GameRoomManager.Instance.GetGameRoom(deltaActorSync.RoomId);
        if (gameRoom == null)
        {
            DeltaActorSyncResponse deltaActorSyncResponse = new DeltaActorSyncResponse
            {
                IsSuccess = false,
                Message = "Room not exist",
            };
            await GetClientHandle().SendMessage(MessageRequestType.SyncActorDetailResponse, deltaActorSyncResponse);
            return;
        }
        gameRoom.RoomWorld.SyncActors(deltaActorSync.PlayerId,deltaActorSync.Actors);
        DeltaActorSyncResponse deltaActorSyncResponseSuc = new DeltaActorSyncResponse
        {
            IsSuccess = false,
            Message = "Room not exist",
        };
        gameRoom.RoomWorld.OptionRoomActor((actor) =>
        {
            if (deltaActorSync.InViewActorIds.Contains(actor.ActorId))
            {
                DeltaActorSyncData deltaActorSyncData = new DeltaActorSyncData
                {
                    ActorId = actor.ActorId,
                    Pos = actor.Pos,
                    Rot = actor.Rot,
                };
                deltaActorSyncResponseSuc.Actors.Add(deltaActorSyncData);
            }
        });
        await GetClientHandle().SendMessage(MessageRequestType.SyncActorDetailResponse, deltaActorSyncResponseSuc);
    }
}