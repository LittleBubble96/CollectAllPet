

using ShareProtobuf;

public class SyncActorAnimationDeltaRequestHandle : MessageRquestBase
{
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        SyncActorAnimationToServerRequest deltaActorSync = await GetClientHandle().ReceiveMessage<SyncActorAnimationToServerRequest>(messageBuffer);
        Console.WriteLine("SyncActorAnimationToServerRequest , RoomId: {0}", deltaActorSync.RoomId);
        SyncActorAnimationToClientRequest deltaActorSyncResponseSuc = new SyncActorAnimationToClientRequest
        {
            Actors = deltaActorSync.Actors,
        };
       
        await GetClientHandle().SendMessage(MessageRequestType.SyncActorDetailResponse, deltaActorSyncResponseSuc);
    }
}