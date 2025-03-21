using ShareProtobuf;

public class RefreshRoomListRequestHandle : MessageRquestBase
{
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        RefreshRoomList refreshRoomList = await GetClientHandle().ReceiveMessage<RefreshRoomList>(messageBuffer);
        Console.WriteLine("RefreshRoomListRequest RefreshRoomList");
        RefreshRoomListResponse refreshRoomListResponse = new RefreshRoomListResponse
        {
            RoomInfos = GameRoomManager.Instance.GetSimpleRoomInfos(),
        };
        await GetClientHandle().SendMessage(MessageRequestType.RefreshRoomListResponse, refreshRoomListResponse);
    }
}