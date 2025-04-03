using System.Collections.Concurrent;
using Newtonsoft.Json;
using ShareProtobuf;

public enum EActorRoleType
{
    None,
    Player,
    NPC,
    Monster,
    Interactive,
    BreakInteractive,
}

public class RoomActor
{ 
    public int ActorId { get; private set; }
    
    public EActorRoleType Role { get; private set; }
    public Vector3 Pos { get; private set; }
    public Vector3 Rot { get; private set; }
    
    public Vector3 Speed { get; private set; }
    
    public long SyncTime { get; set; }

    
    public string OwnerPlayerId { get; private set; }
    
    public int ActorCfgId { get; private set; }
    
    public string ActorName { get; private set; }
    
    protected ConcurrentDictionary<int,object> ActorAttributes = new ConcurrentDictionary<int, object>();
    //脏属性类型
    protected int dirtyAttributeType = -1;

    public void Init(string playerId , EActorRoleType roleType, int actorCfgId ,int actorId, Vector3 pos, Vector3 rot)
    {
        ActorId = actorId;
        Role = roleType;
        Pos = pos;
        Rot = rot;
        OwnerPlayerId = playerId;
        ActorCfgId = actorCfgId;
        ActorName = GetActorName();
        OnInit();
    }


    private string GetActorName()
    {
        string preName = "";
        switch (Role)
        {
            case EActorRoleType.Player:
                preName = "Player";
                break;
            case EActorRoleType.NPC:
                preName = "NPC";
                break;
            case EActorRoleType.Monster:
                preName = "Monster";
                break;
            case EActorRoleType.Interactive:
                preName = "Interactive";
                break;
        }
        return preName + "_" + ActorCfgId + "_" + ActorId + "_" + OwnerPlayerId;
    }
    
    
    public void SyncPos(Vector3 pos)
    {
        Pos = pos;
    }
    
    public void SyncRot(Vector3 rot)
    {
        Rot = rot;
    }
    
    public void SyncSpeed(Vector3 speed)
    {
        Speed = speed;
    }
    
    public void SyncServeTime()
    {
        SyncTime = DateTime.UtcNow.Ticks;
    }
    
    public float GetDistance(RoomActor actor)
    {
        return Vector3.Distance(Pos, actor.Pos);
    }

    public string GetDirtyAttributeJson()
    {
        Dictionary<int,object> d = new Dictionary<int,object>();
        foreach (var attribute in ActorAttributes)
        {
            if ((attribute.Key & dirtyAttributeType)  > 0)
            {
                //TODO 这里需要序列化成json
                d [attribute.Key] = attribute.Value;
            }
        }
        return JsonConvert.SerializeObject(d);
    }
    
    public string GetAllDirtyAttributeJson()
    {
        return JsonConvert.SerializeObject(ActorAttributes);
    }

    public virtual void OnInit()
    {
        
    }
    
    
    #region 属性
    public void AddAttribute(int attributeId, object value)
    {
        //添加属性
        ActorAttributes.TryAdd(attributeId, value);
    }
    
    public int GetIntAttribute(int attributeId)
    {
        if (ActorAttributes.TryGetValue(attributeId, out object value))
        {
            return Convert.ToInt32(value);
        }
        return 0;
    }
    
    public float GetFloatAttribute(int attributeId)
    {
        if (ActorAttributes.TryGetValue(attributeId, out object value))
        {
            return Convert.ToSingle(value);
        }
        return 0;
    }
    
    //更新属性 并标记为脏
    public void UpdateAttribute(int attributeId, object value)
    {
        //更新属性
        ActorAttributes[attributeId] = value;
        dirtyAttributeType = dirtyAttributeType | attributeId;
    }
    
    public bool RoomWorldAttIsDirty()
    {
        return dirtyAttributeType > 0;
    }
    

    #endregion

}
public class RoomWorld
{
    public int RoomId { get; private set; }
    public ConcurrentDictionary<int, RoomActor> Actors = new ConcurrentDictionary<int, RoomActor>();

    private int generateActorId = 0;
    private int maxActorCount = 1000;

    public void Init(int roomId)
    {
        RoomId = roomId;
    }
    
    public RoomActor GetActor(int actorId)
    {
        if (Actors.TryGetValue(actorId, out RoomActor actor))
        {
            return actor;
        }
        return null;
    }
    
    public PetActor GetPet(int actorId)
    {
        if (Actors.TryGetValue(actorId, out RoomActor actor))
        {
            if (actor is PetActor petActor)
            {
                return petActor;
            }
        }
        return null;
    }
    
    public BreakInteractiveActor GetBreakInteractive(int actorId)
    {
        if (Actors.TryGetValue(actorId, out RoomActor actor))
        {
            if (actor is BreakInteractiveActor breakInteractiveActor)
            {
                return breakInteractiveActor;
            }
        }
        return null;
    }

    public CreateActorResultCallBack AddActor(string playerId ,EActorRoleType roleType, int actorCfgId , Vector3 pos, Vector3 rot)
    {
        RoomActor actor;
        if (roleType == EActorRoleType.BreakInteractive)
        {
            actor = new BreakInteractiveActor();
        }
        else if (roleType == EActorRoleType.Monster)
        {
            actor = new PetActor();
        }
        else
        {
            actor = new RoomActor();
        }
        CreateActorResultCallBack result = GenerateActorId();
        if (!result.IsSuccess)
        {
            return result;
        }
        actor.Init(playerId,roleType,actorCfgId,result.ActorId, pos, rot);
        Actors.TryAdd(result.ActorId, actor);
        return result;
    }

    public CreateActorResultCallBack GenerateActorId()
    {
        generateActorId++;
        int loopCount = 0;
        while (Actors.ContainsKey(generateActorId))
        {
            generateActorId++;
            if (generateActorId > maxActorCount)
            {
                generateActorId = 0;
                loopCount++;
                if (loopCount > 1)
                {
                    Console.WriteLine("Actor已满");
                    return new CreateActorResultCallBack() { IsSuccess = false, Message = "Actor已满" };
                }
            }
        }
        return new CreateActorResultCallBack() { IsSuccess = true, ActorId = generateActorId };
    }

    public List<GameActorInfo> GetActors()
    {
        List<GameActorInfo> gameActorInfos = new List<GameActorInfo>();
        foreach (var actor in Actors)
        {
            gameActorInfos.Add(new GameActorInfo()
            {
                OwnerPlayerId = actor.Value.OwnerPlayerId, 
                ActorConfigId = actor.Value.ActorCfgId, 
                RefActorId = actor.Value.ActorId , 
                ActorName = actor.Value.ActorName,
                ActorRoleType = (int)actor.Value.Role,
                SpawnPos = actor.Value.Pos,
                SpawnRot = actor.Value.Rot,
            });
        }
        return gameActorInfos;
    }
    
    public List<GameActorInfo> GetActors(List<int> actorIds)
    {
        List<GameActorInfo> gameActorInfos = new List<GameActorInfo>();
        foreach (var actorId in actorIds)
        {
            GameActorInfo actorInfo = GetActorInfo(actorId);
            if (actorInfo != null)
            {
                gameActorInfos.Add(actorInfo);
            }
        }
        return gameActorInfos;
    }

    public GameActorInfo GetActorInfo(int actorId)
    {
        if (Actors.TryGetValue(actorId, out RoomActor actor))
        {
            return new GameActorInfo()
            {
                OwnerPlayerId = actor.OwnerPlayerId, 
                ActorConfigId = actor.ActorCfgId, 
                RefActorId = actor.ActorId , 
                ActorName = actor.ActorName,
                ActorRoleType = (int)actor.Role,
                SpawnPos = actor.Pos,
                SpawnRot = actor.Rot,
            };
        }
        return null;
    }
    
    public RoomActor GetRoomActorByPlayerId(string playerId)
    {
        foreach (var actor in Actors)
        {
            if (actor.Value.OwnerPlayerId == playerId)
            {
                return actor.Value;
            }
        }
        return null;
    }
    
    public void SyncActors(string playerId, List<DeltaActorSyncData> actors)
    {
        foreach (var actor in actors)
        {
            if (Actors.TryGetValue(actor.ActorId, out RoomActor roomActor))
            {
                roomActor.SyncPos(actor.Pos);
                roomActor.SyncRot(actor.Rot);
                roomActor.SyncSpeed(actor.Speed);
                roomActor.SyncServeTime();
            }
        }
    }

    public void OptionRoomActor(Action<RoomActor> action)
    {
        foreach (var actor in Actors)
        {
            action(actor.Value);
        }
    }
    
    public bool RoomWorldAttIsDirty()
    {
        foreach (var actor in Actors)
        {
            if (actor.Value.RoomWorldAttIsDirty())
            {
                return true;
            }
        }
        return false;
    }
    
    
}