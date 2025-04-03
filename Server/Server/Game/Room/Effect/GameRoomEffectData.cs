
using ShareProtobuf;

public class GameRoomEffectData : IRecycle
{
    public string EffectName { get; set; }
    public Vector3 Position { get; set; }
    public Vector3 Rotation { get; set; }
    public bool IsLoop { get; set; }
    public int ActorId { get; set; }
    public string ActorSocket { get; set; }
    
    public void Recycle()
    {
        EffectName = string.Empty;
        Position = new Vector3();
        Rotation = new Vector3();
        IsLoop = false;
        ActorId = -1;
        ActorSocket = string.Empty;
    }
}