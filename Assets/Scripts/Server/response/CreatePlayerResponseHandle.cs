

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
            RoomManager.Instance.UpdateDetailRoom(response.RoomDetailInfo,response.RefActorId);
            GameManager.Instance.StartCoroutine(RoomManager.Instance.LoadSceneActor(null));
        }

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