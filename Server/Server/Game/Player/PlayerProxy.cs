using ShareProtobuf;

public class PlayerProxy
{
    public string PlayerId { get; set; }
    public string PlayerClientEndPoint { get; set; }

    public PlayerData PlayerData { get; set; }
    public PlayerProxy()
    {
    }


}