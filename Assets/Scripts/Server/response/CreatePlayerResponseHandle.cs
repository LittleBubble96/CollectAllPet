

using System.Threading.Tasks;
using ShareProtobuf;
using UnityEngine;

public class CreatePlayerResponseHandle : ClientMessageRequestBase
{
    public CreatePlayerResponseHandle()
    {
        
    }

    public override async Task HandleResponse(MessageRequestType msgResponseType, byte[] messageBuffer)
    {
        CreatePlayerActorResponse response = await GameManager.GetNetworkManager().ReceiveMessage<CreatePlayerActorResponse>(messageBuffer);
        Debug.Log("CreatePlayerActorResponse HandleResponse: " + response.Message);
        if (response.IsSuccess)
        {
            Debug.Log("Create room success");
            //创建刷新
            RoomManager.Instance.UpdateHostActorId(response.RefActorId);
        }

    }

    public override MessageRequestType GetRequestMessageType()
    {
        return MessageRequestType.CreateActorRequest;
    }

    public override MessageRequestType GetResponseMessageType()
    {
        return MessageRequestType.CreateActorResponse;
    }
}