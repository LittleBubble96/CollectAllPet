using ShareProtobuf;

public class PlayerProxy
{
    public int PlayerId { get; set; }
    public string PlayerClientEndPoint { get; set; }

    public PlayerData PlayerData { get; set; }
    public PlayerProxy()
    {
    }

    #region 宠物逻辑

    //背包添加一个宠物
    public async Task AddPet(int petConfig)
    {
        int petId = await DBModule.Instance.GetDbModule<CharacterDBService>().AddPet(PlayerData.userId, petConfig, false);
        if (petId < 0)
        {
            return;
        }
        PlayerPetData petData = new PlayerPetData();
        petData.petId = petId;
        petData.petConfigId = petConfig;
        petData.petName = "Pet";
        petData.bBattle = false;
        PlayerData.playerPetDatas.Add(petData);
    }

    #endregion

}