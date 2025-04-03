using System.Collections.Concurrent;
using ShareProtobuf;

public enum EActorRoleType
{
    None,
    Player,
    Monster,
    Interactive,
    BreakInteractive,
}

public class RoomWorld
{
    public int RoomId { get; private set; }
    public ConcurrentDictionary<int, RoomActor> Actors = new ConcurrentDictionary<int, RoomActor>();
    public ConcurrentDictionary<int,RoomActor> WaitDestroyActors = new ConcurrentDictionary<int, RoomActor>();

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
        actor.Init(playerId,this,roleType,actorCfgId,result.ActorId, pos, rot);
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


    public void DestroyActor(RoomActor actor)
    { 
        if (Actors.TryRemove(actor.ActorId, out _))
        {
            WaitDestroyActors.TryAdd(actor.ActorId, actor);
        }
    }
    
}