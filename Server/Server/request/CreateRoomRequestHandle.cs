
using ShareProtobuf;

public class CreateRoomRequestHandle : MessageRquestBase
{
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        CreateRoomRequest createRoomRequest = await GetClientHandle().ReceiveMessage<CreateRoomRequest>(messageBuffer);
        Console.WriteLine("CreateRoomRequest RoomName: {0}", createRoomRequest.RoomName);
        CreateRoomResultCallBack result = GameRoomManager.Instance.CreateRoom(createRoomRequest.PlayerId,GetClientHandle().ClientRemoteEndPoint, createRoomRequest.RoomName , createRoomRequest.MaxPlayerCount);
        
        CreateRoomResponse createRoomResponse = new CreateRoomResponse
        {
            RoomId= result.RoomId,
            IsSuccess = result.IsSuccess,
            Message = result.Message,
        };
        await GetClientHandle().SendMessage(MessageRequestType.CreateRoomResponse, createRoomResponse);
    }
}