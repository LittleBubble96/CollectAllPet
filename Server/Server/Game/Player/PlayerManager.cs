using ShareProtobuf;
using System.Collections.Concurrent;

public class PlayerManager : Singleton<PlayerManager>
{
    public ConcurrentDictionary<int, PlayerProxy> Players;

    public void Init()
    {
        Players = new ConcurrentDictionary<int, PlayerProxy>();
    }

    public void AddPlayer(string PlayerClientEndPoint, PlayerData playerData)
    {
        PlayerProxy player = new PlayerProxy();
        player.PlayerId = playerData.userId;
        player.PlayerClientEndPoint = PlayerClientEndPoint;
        player.PlayerData = playerData;
        AddPlayer_Internal(player);
    }

    public PlayerProxy GetPlayer(int playerId)
    {
       if (Players.TryGetValue(playerId, out PlayerProxy player))
       {
           return player;
       }
       return null;
    }
    
    public async Task<ResultCallBack> AddPet(int playerId, int petConfigId)
    {
        if (Players.TryGetValue(playerId, out PlayerProxy player))
        {
            await player.AddPet(petConfigId);
            return ResultCallBack.Success();
        }
        return ResultCallBack.Failed();
    }

    private void AddPlayer_Internal(PlayerProxy player)
    {
        Players.TryAdd(player.PlayerId, player);
    }
}