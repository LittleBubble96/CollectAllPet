
using ShareProtobuf;

public class SyncActorDeltaRequestHandle : MessageRquestBase
{
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        DeltaActorSyncRequest deltaActorSync = await GetClientHandle().ReceiveMessage<DeltaActorSyncRequest>(messageBuffer);
        Console.WriteLine("DeltaActorSyncRequest PlayerId: {0} , RoomId: {1}", deltaActorSync.PlayerId, deltaActorSync.RoomId);
        GameRoom gameRoom = GameRoomManager.Instance.GetGameRoom(deltaActorSync.RoomId);
        if (gameRoom == null || deltaActorSync.InViewActorIds.Count != deltaActorSync.ActorStates.Length)
        {
            DeltaActorSyncResponse deltaActorSyncResponse = new DeltaActorSyncResponse
            {
                IsSuccess = false,
                Message = "Room not exist Or ActorStates count not match",
            };
            await GetClientHandle().SendMessage(MessageRequestType.SyncActorDetailResponse, deltaActorSyncResponse);
            return;
        }
        gameRoom.SyncActors(deltaActorSync.PlayerId,deltaActorSync.Actors);
        DeltaActorSyncResponse deltaActorSyncResponseSuc = new DeltaActorSyncResponse
        {
            IsSuccess = true,
            Actors = new List<DeltaActorSyncData>(),
        };
        for (int i = 0; i < deltaActorSync.InViewActorIds.Count; i++)
        {
            int actorId = deltaActorSync.InViewActorIds[i];
            RoomActor actor = gameRoom.RoomWorld.GetActor(actorId);
            if (actor == null)
            {
                DeltaActorSyncData deltaActorSyncData = new DeltaActorSyncData
                {
                    ActorId = actor.ActorId,
                    Pos = actor.Pos,
                    Rot = actor.Rot,
                    Speed = actor.Speed,
                    SyncTime = actor.SyncTime,
                    UpdateAttribute = deltaActorSync.ActorStates[i] == 1 ? actor.GetAllDirtyAttributeJson() : "",
                };
                deltaActorSyncResponseSuc.Actors.Add(deltaActorSyncData);
            }
        }
        await GetClientHandle().SendMessage(MessageRequestType.SyncActorDetailResponse, deltaActorSyncResponseSuc);
    }
}