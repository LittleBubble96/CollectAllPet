using ShareProtobuf;

public class RoomPlayer
{
    public string PlayerId { get; private set; }
    public int RoomId { get; private set; }
    public Vector3 SpawnPos { get; private set; }
    public Vector3 SpawnRot { get; private set; }
    
    public string ClientIPAndPort { get; private set; }

    public void Init(string playerId,string clientIPAndPort, int roomId , Vector3 spawnPos, Vector3 spawnRot)
    {
        PlayerId = playerId;
        RoomId = roomId;
        SpawnPos = spawnPos;
        SpawnRot = spawnRot;
        ClientIPAndPort = clientIPAndPort;
    }

    public SimplePlayerInfo GetSimplePlayerInfo()
    {
        SimplePlayerInfo simplePlayerInfo = new SimplePlayerInfo();
        PlayerProxy playerProxy = PlayerManager.Instance.GetPlayer(PlayerId);
        if (playerProxy == null)
        {
            return simplePlayerInfo;
        }
        simplePlayerInfo.UserId = playerProxy.PlayerData.userId;
        simplePlayerInfo.UserName = playerProxy.PlayerData.userName;
        return simplePlayerInfo;
    }
}