
using System.Threading.Tasks;
using ShareProtobuf;
using UnityEngine;

public class CreateRoomResponseHandle : ClientMessageRequestBase
{
    public CreateRoomResponseHandle()
    {
        
    }

    public override async Task HandleResponse(MessageRequestType msgResponseType, byte[] messageBuffer)
    {
        CreateRoomResponse response = await GameManager.GetNetworkManager().ReceiveMessage<CreateRoomResponse>(messageBuffer);
        Debug.Log("LoginResponse HandleResponse: " + response.Message);
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