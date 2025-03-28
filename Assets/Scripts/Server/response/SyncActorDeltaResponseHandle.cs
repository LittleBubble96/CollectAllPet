
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
        Debug.Log("SyncActorDeltaResponseHandle HandleResponse: " + response.Message);
        if (!response.IsSuccess)
        {
            Debug.Log("SyncActorDeltaResponseHandle HandleResponse: " + response.Message);
            return;
        }
        //设置actor信息
        RoomManager.Instance.SyncServerActorInfo(response.Actors);
        GameManager.GetGameSyncActorManager().ReceiveSyncActorDeltaResponse();
    }

    public override MessageRequestType GetRequestMessageType()
    {
        return MessageRequestType.SyncActorDetailRequest;
    }

    public override MessageRequestType GetResponseMessageType()
    {
        return MessageRequestType.SyncActorDetailResponse;
    }
}