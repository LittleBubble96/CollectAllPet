
using System.Collections.Concurrent;
using ShareProtobuf;

public class GameRoomEffectController
{
    public ConcurrentDictionary<int, GameRoomEffectData> EffectDataList { get; private set; } = new ConcurrentDictionary<int, GameRoomEffectData>();

    protected GameRoom Room;
    protected int generateEffectId = 0;
    protected int maxEffectCount = 1000;
    
    protected Task playEffectTask = null;
    
    public GameRoomEffectController(GameRoom room)
    {
        this.Room = room;
    }
    
    public void PlayEffect(int actorId , string effectName, Vector3 position, Vector3 rotation , bool isLoop = false, string actorSocket = "")
    {
        GameRoomEffectData effectData = new GameRoomEffectData()
        {
            EffectName = effectName,
            Position = position,
            Rotation = rotation,
            IsLoop = isLoop,
            ActorId = actorId,
            ActorSocket = actorSocket
        };
        
        int effectId = GetGenerateEffectId();
        EffectDataList.TryAdd(effectId, effectData);
    }
    
    public void DoFixedUpdate()
    {
        if (playEffectTask == null || playEffectTask.IsCompleted)
        {
            playEffectTask = PlayEffectAsync();
        }
    }
    
    protected async Task PlayEffectAsync()
    {
        if (EffectDataList.Count > 0)
        {
            SyncPlayEffectToClientRequest effectRequest = ServerFactory.Instance.GetPlayEffectDataFactory().GetObject();
            
            foreach (var effect in EffectDataList)
            {
                SyncPlayEffectToClientData effectData = ServerFactory.Instance.GetPlayEffectData().GetObject();
                effectData.EffectName = effect.Value.EffectName;
                effectData.Position = effect.Value.Position;
                effectData.Rotation = effect.Value.Rotation;
                effectData.IsLoop = effect.Value.IsLoop;
                effectData.ActorId = effect.Value.ActorId;
                effectData.ActorSocket = effect.Value.ActorSocket;
                effectRequest.EffectActors.Add(effectData);
            }
            ClearEffectData();
            await Room.SendMessageToAllClient(MessageRequestType.PlayEffectRequestToClient, effectRequest);
            ServerFactory.Instance.GetPlayEffectDataFactory().PutObject(effectRequest);
            foreach (var effect in effectRequest.EffectActors)
            {
                ServerFactory.Instance.GetPlayEffectData().PutObject(effect);
            }
        }
        
    }
    
    protected void ClearEffectData()
    {
        foreach (var effect in EffectDataList)
        {
            ServerFactory.Instance.GetGameRoomEffectDataFactory().PutObject(effect.Value);
        }
        EffectDataList.Clear();
    }
    
    protected int GetGenerateEffectId()
    {
        generateEffectId++;
        if (generateEffectId >= maxEffectCount)
        {
            generateEffectId = 0;
        }
        return generateEffectId;
    }
}