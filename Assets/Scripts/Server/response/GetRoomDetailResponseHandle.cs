
using System.Threading.Tasks;
using ShareProtobuf;
using UnityEngine;

public class GetRoomDetailResponseHandle : ClientMessageRequestBase
{
    public GetRoomDetailResponseHandle()
    {
        
    }

    public override async Task HandleResponse(MessageRequestType msgResponseType, byte[] messageBuffer)
    {
        GetRoomDetailResponse response = await GameManager.GetNetworkManager().ReceiveMessage<GetRoomDetailResponse>(messageBuffer);
        Debug.Log(" GetRoomDetailResponseHandle RoomId: " + response.RoomDetailInfo.RoomId + " RoomName: " + response.RoomDetailInfo.RoomName);
        if (response.IsSuccess)
        {
            Debug.Log("Create room success");
        }
        GameManager.GetAppEventDispatcher().BroadcastListener(EventName.Event_CreateRoomSuccess,response.IsSuccess, response.Message);

    }

    public override MessageRequestType GetRequestMessageType()
    {
        return MessageRequestType.CreateRoomRequest;
    }

    public override MessageRequestType GetResponseMessageType()
    {
        return MessageRequestType.CreateRoomResponse;
    }
}