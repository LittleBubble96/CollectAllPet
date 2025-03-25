

using ShareProtobuf;

public class JoinRoomRequestHandle : MessageRquestBase
{
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        JoinRoomRequest joinRoomRequest = await GetClientHandle().ReceiveMessage<JoinRoomRequest>(messageBuffer);
        Console.WriteLine("JoinRoomRequest RoomName: {0}", joinRoomRequest.RoomId);
        ResultCallBack result = GameRoomManager.Instance.JoinRoom(joinRoomRequest.PlayerId , GetClientHandle().ClientRemoteEndPoint, joinRoomRequest.RoomId);

        JoinRoomResponse joinRoomResponse = new JoinRoomResponse
        {
            RoomId = joinRoomRequest.RoomId,
            IsSuccess = result.IsSuccess,
            Message = result.Message,
        };
        await GetClientHandle().SendMessage(MessageRequestType.JoinRoomResponse, joinRoomResponse);
    }
}