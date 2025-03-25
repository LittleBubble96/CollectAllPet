using System.Threading.Tasks;
using ShareProtobuf;
using UnityEngine;

public class JoinRoomResponseHandle : ClientMessageRequestBase
{
    public JoinRoomResponseHandle()
    {
        
    }

    public override async Task HandleResponse(MessageRequestType msgResponseType, byte[] messageBuffer)
    {
        JoinRoomResponse response = await GameManager.GetNetworkManager().ReceiveMessage<JoinRoomResponse>(messageBuffer);
        if (response.IsSuccess)
        {
            Debug.Log("Join room success");
            RoomManager.Instance.EnterRoom(response.RoomId);
            GameManager.GetGameStateMachine().ChangeGameState(GameStateEnum.RoomGame);
        }
        GameManager.GetAppEventDispatcher().BroadcastListener(EventName.Event_JoinRoomSuccess,response.RoomId, response.IsSuccess);

    }

    public override MessageRequestType GetRequestMessageType()
    {
        return MessageRequestType.JoinRoomRequest;
    }

    public override MessageRequestType GetResponseMessageType()
    {
        return MessageRequestType.JoinRoomResponse;
    }
}