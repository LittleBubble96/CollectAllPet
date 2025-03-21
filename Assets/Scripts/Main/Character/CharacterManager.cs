using ShareProtobuf;

public class CharacterManager : Singleton<CharacterManager>
{
     public ClientPlayerInfo PlayerInfo { get; set; }
     
     public void UpdatePlayerInfo(PlayerData playerInfo)
     {
          if (PlayerInfo == null)
          {
               PlayerInfo = new ClientPlayerInfo();
          }
          PlayerInfo.PlayerId = playerInfo.userId;
          PlayerInfo.PlayerName = playerInfo.userName;
     }
}