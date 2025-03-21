using System.Net.Sockets;
using System.Threading.Tasks;
using ShareProtobuf;
using UnityEngine;

public class LoginRequest : ClientMessageRequestBase
{
    public LoginRequest()
    {
        
    }

    public override async Task HandleResponse(MessageRequestType msgResponseType, byte[] messageBuffer)
    {
        PlayerLoginResponse playerLoginResponse = await GameManager.GetNetworkManager().ReceiveMessage<PlayerLoginResponse>(messageBuffer);
        Debug.Log("LoginResponse HandleResponse: " + playerLoginResponse.Message);
        if (playerLoginResponse.IsSuccess)
        {
            GameManager.GetGameStateMachine().ChangeGameState(GameStateEnum.Main);
            CharacterManager.Instance.UpdatePlayerInfo(playerLoginResponse.PlayerData);
        }
    }

    public override MessageRequestType GetRequestMessageType()
    {
        return MessageRequestType.PlayerLogin;
    }

    public override MessageRequestType GetResponseMessageType()
    {
        return MessageRequestType.PlayerLoginResponse;
    }
}