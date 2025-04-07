

using System.Threading.Tasks;
using ShareProtobuf;
using UnityEngine;

public class PetAttackTargetResponseHandle : ClientMessageRequestBase
{
    public PetAttackTargetResponseHandle()
    {
        
    }

    public override async Task HandleResponse(MessageRequestType msgResponseType, byte[] messageBuffer)
    {
        PetAttackTargetActorResponse response = await GameManager.GetNetworkManager().ReceiveMessage<PetAttackTargetActorResponse>(messageBuffer);
        Debug.Log("[Attack]PetAttackTargetActorResponse HandleResponse: " + response.IsSuccess);
        if (response.IsSuccess)
        {
            Debug.Log("PetAttackTargetActorResponse  find success");
        }
    }

    public override MessageRequestType GetRequestMessageType()
    {
        return MessageRequestType.PetAttackTargetRequest;
    }

    public override MessageRequestType GetResponseMessageType()
    {
        return MessageRequestType.PetAttackTargetResponse;
    }
}