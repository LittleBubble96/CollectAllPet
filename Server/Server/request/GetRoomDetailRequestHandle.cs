

using ShareProtobuf;

public class GetRoomDetailRequestHandle : MessageRquestBase
{
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        GetRoomDetailRequest getRoomRequest = await GetClientHandle().ReceiveMessage<GetRoomDetailRequest>(messageBuffer);
        Console.WriteLine("GetRoomDetailRequest RoomId: {0}", getRoomRequest.RoomId , "PlayerId: {1}", getRoomRequest.PlayerId);
        GameRoom gameRoom = GameRoomManager.Instance.GetGameRoom(getRoomRequest.RoomId);
        if (gameRoom == null)
        {
            GetRoomDetailResponse getRoomDetailResponse = new GetRoomDetailResponse
            {
                IsSuccess = false,
                Message = "房间不存在",
            };
            await GetClientHandle().SendMessage(MessageRequestType.GetRoomDetailResponse, getRoomDetailResponse);
            return;
        }
        GetRoomDetailResponse getRoomDetailResponseSuc = new GetRoomDetailResponse
        {
            IsSuccess = true,
            Message = "获取房间信息成功",
            RoomDetailInfo = gameRoom.GetRoomDetailInfo(),
            RefActorId = gameRoom.GetRoomActorByPlayerId(getRoomRequest.PlayerId).ActorId,
        };
        await GetClientHandle().SendMessage(MessageRequestType.GetRoomDetailResponse, getRoomDetailResponseSuc);
    }
}