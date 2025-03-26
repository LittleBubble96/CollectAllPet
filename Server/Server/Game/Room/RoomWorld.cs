using System.Collections.Concurrent;
using System.Numerics;
using ShareProtobuf;

public enum EActorRoleType
{
    None,
    Player,
    NPC,
    Monster,
    Interactive,
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
    
    public string ActorRes { get; private set; }
    
    public string ActorName { get; private set; }

    public void Init(string playerId , EActorRoleType roleType, string actorRes ,int actorId, Vector3 pos, Vector3 rot)
    {
        ActorId = actorId;
        Role = roleType;
        Pos = pos;
        Rot = rot;
        OwnerPlayerId = playerId;
        ActorRes = actorRes;
        ActorName = ActorRes + "_" + ActorId + "_" + OwnerPlayerId;
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

    public CreateActorResultCallBack AddActor(string playerId ,EActorRoleType roleType, string actorRes , Vector3 pos, Vector3 rot)
    {
        RoomActor actor = new RoomActor();
        CreateActorResultCallBack result = GenerateActorId();
        if (!result.IsSuccess)
        {
            return result;
        }
        actor.Init(playerId,roleType,actorRes,result.ActorId, pos, rot);
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
        return new CreateActorResultCallBack() { IsSuccess = true };
    }

    public List<GameActorInfo> GetActors()
    {
        List<GameActorInfo> gameActorInfos = new List<GameActorInfo>();
        foreach (var actor in Actors)
        {
            gameActorInfos.Add(new GameActorInfo()
            {
                OwnerPlayerId = actor.Value.OwnerPlayerId, 
                ActorRes = actor.Value.ActorRes, 
                RefActorId = actor.Value.ActorId , 
                ActorName = actor.Value.ActorName,
                ActorRoleType = (int)actor.Value.Role,
            });
        }
        return gameActorInfos;
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
}