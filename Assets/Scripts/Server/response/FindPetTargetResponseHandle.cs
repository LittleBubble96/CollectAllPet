

using System.Threading.Tasks;
using ShareProtobuf;
using UnityEngine;

public class FindPetTargetResponseHandle : ClientMessageRequestBase
{
    public FindPetTargetResponseHandle()
    {
        
    }

    public override async Task HandleResponse(MessageRequestType msgResponseType, byte[] messageBuffer)
    {
        FindPetTargetActorIdResponse response = await GameManager.GetNetworkManager().ReceiveMessage<FindPetTargetActorIdResponse>(messageBuffer);
        Debug.Log("FindPetTargetActorIdResponse HandleResponse: " + response.IsSuccess);
        if (response.IsSuccess)
        {
            Debug.Log("FindPetTargetActorIdResponse  find success");
        }
        RoomManager.Instance.SetActorTarget(response.PetActorId,response.TargetActorId);

    }

    public override MessageRequestType GetRequestMessageType()
    {
        return MessageRequestType.FindPetTargetRequest;
    }

    public override MessageRequestType GetResponseMessageType()
    {
        return MessageRequestType.FindPetTargetResponse;
    }
}