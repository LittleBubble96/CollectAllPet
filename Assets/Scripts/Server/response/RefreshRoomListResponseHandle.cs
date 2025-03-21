
using System.Collections.Generic;
using System.Threading.Tasks;
using ShareProtobuf;
using UnityEngine;

public class RefreshRoomListResponseHandle : ClientMessageRequestBase
{
    public RefreshRoomListResponseHandle()
    {
        
    }

    public override async Task HandleResponse(MessageRequestType msgResponseType, byte[] messageBuffer)
    {
        Debug.Log("pre RefreshRoomListResponseHandle HandleResponse");
        RefreshRoomListResponse response = await GameManager.GetNetworkManager().ReceiveMessage<RefreshRoomListResponse>(messageBuffer);
        Debug.Log("post RefreshRoomListResponseHandle HandleResponse");
        List<SimpleRoomInfo> roomInfos = response.RoomInfos;
        roomInfos = roomInfos ?? new List<SimpleRoomInfo>();
        GameManager.GetAppEventDispatcher().BroadcastListener(EventName.EVENT_RefreshRoom, roomInfos);
        Debug.Log("last RefreshRoomListResponseHandle HandleResponse");
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