
using System.Collections.Generic;
using System.Threading.Tasks;
using ShareProtobuf;
using UnityEngine;

public class SyncActorDeltaResponseHandle : ClientMessageRequestBase
{
    public SyncActorDeltaResponseHandle()
    {
        
    }

    public override async Task HandleResponse(MessageRequestType msgResponseType, byte[] messageBuffer)
    {
        DeltaActorSyncResponse response = await GameManager.GetNetworkManager().ReceiveMessage<DeltaActorSyncResponse>(messageBuffer);
        if (!response.IsSuccess)
        {
            Debug.Log("SyncActorDeltaResponseHandle HandleResponse: " + response.Message);
            return;
        }
        
    }

    public override MessageRequestType GetRequestMessageType()
    {
        return MessageRequestType.RefreshRoomList;
    }

    public override MessageRequestType GetResponseMessageType()
    {
        return MessageRequestType.RefreshRoomListResponse;
    }
}