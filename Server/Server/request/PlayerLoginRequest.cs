using ProtoBuf;
using ShareProtobuf;
using System.IO;
using System.Net.Sockets;

public class PlayerLoginRequest : MessageRquestBase
{
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        // 读取数据
        PlayerLogin playerLogin = await GetClientHandle().ReceiveMessage<PlayerLogin>(messageBuffer);
        // 处理数据
        Console.WriteLine("PlayerLoginRequest Account: {0}, Password: {1}", playerLogin.Account, playerLogin.Password);
        // 返回数据
        PlayerData playerData = new PlayerData
        {
            userId = playerLogin.Account,
            userName = playerLogin.Account,
            playerConfigId = 1,
        };

        PlayerLoginResponse playerLoginResponse = new PlayerLoginResponse
        {
            IsSuccess = true,
            Message = "登录成功",
            PlayerData = playerData,
        };
        PlayerManager.Instance.AddPlayer(GetClientHandle().ClientRemoteEndPoint, playerData);
        await GetClientHandle().SendMessage(MessageRequestType.PlayerLoginResponse, playerLoginResponse);
        Console.WriteLine("PlayerLoginRequest PlayerLoginResponse");
    }
}