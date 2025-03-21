using ShareProtobuf;
using System.Collections.Concurrent;

public class PlayerManager : Singleton<PlayerManager>
{
    public ConcurrentDictionary<string, PlayerProxy> Players;

    public void Init()
    {
        Players = new ConcurrentDictionary<string, PlayerProxy>();
    }

    public void AddPlayer(string PlayerClientEndPoint, PlayerData playerData)
    {
        PlayerProxy player = new PlayerProxy();
        player.PlayerId = playerData.userId;
        player.PlayerClientEndPoint = PlayerClientEndPoint;
        player.PlayerData = playerData;
        AddPlayer_Internal(player);
    }

    public PlayerProxy GetPlayer(string playerId)
    {
       if (Players.TryGetValue(playerId, out PlayerProxy player))
       {
           return player;
       }
       return null;
    }

    private void AddPlayer_Internal(PlayerProxy player)
    {
        Players.TryAdd(player.PlayerId, player);
    }
}