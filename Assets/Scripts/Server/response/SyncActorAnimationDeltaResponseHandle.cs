

using System.Collections.Generic;
using System.Threading.Tasks;
using ShareProtobuf;
using UnityEngine;

public class SyncActorAnimationDeltaResponseHandle : ClientMessageRequestBase
{
    public SyncActorAnimationDeltaResponseHandle()
    {
        
    }

    public override async Task HandleResponse(MessageRequestType msgResponseType, byte[] messageBuffer)
    {
        SyncActorAnimationToServerResponse response = await GameManager.GetNetworkManager().ReceiveMessage<SyncActorAnimationToServerResponse>(messageBuffer);
        Debug.Log("SyncActorAnimationToServerResponse HandleResponse: " + response.Message);
        if (!response.IsSuccess)
        {
            Debug.Log("SyncActorAnimationToServerResponse HandleResponse: " + response.Message);
            return;
        }
    }

    public override MessageRequestType GetRequestMessageType()
    {
        return MessageRequestType.SyncActorAnimationDeltaRequest;
    }

    public override MessageRequestType GetResponseMessageType()
    {
        return MessageRequestType.SyncActorAnimationDeltaResponse;
    }
}