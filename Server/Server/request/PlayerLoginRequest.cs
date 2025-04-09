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
        LoginUIdResultCallBack result = await DBModule.Instance.GetDbModule<CharacterDBService>().Login(playerLogin.Account, playerLogin.Password);
        if (!result.IsSuccess)
        {
            PlayerLoginResponse response = new PlayerLoginResponse
            {
                IsSuccess = false,
                Message = result.Message,
            };
            await GetClientHandle().SendMessage(MessageRequestType.PlayerLoginResponse, response);
            Console.WriteLine("PlayerLoginRequest PlayerLoginResponse");
            return;
        }
        //登录成功
        PlayerDB playerDb = await DBModule.Instance.GetDbModule<CharacterDBService>().GetCharacterInfo(result.UId);
        //从数据库里读取
        PlayerData playerData = PlayerDBConvertPlayerData(playerDb);

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

    public PlayerData PlayerDBConvertPlayerData(PlayerDB playerDB)
    {
        PlayerData playerData = new PlayerData
        {
            userId = playerDB.PlayerId,
            userName = playerDB.Name,
            playerConfigId = 1,
            playerPetDatas = new List<PlayerPetData>()
        };
        for (int i = 0; i < playerDB.Pets.Count; i++)
        {
            playerData.playerPetDatas.Add(PetDBConvertPlayerPetData(playerDB.Pets[i]));
        }
        return playerData;
    }
    
    public PlayerPetData PetDBConvertPlayerPetData(PetDB pet)
    {
        PlayerPetData playerPetData = new PlayerPetData
        {
            petId = pet.Id,
            petName ="Pet",
            petConfigId = pet.PetConfigId,
            bBattle = pet.IsBattle,
        };
        return playerPetData;
    }
}