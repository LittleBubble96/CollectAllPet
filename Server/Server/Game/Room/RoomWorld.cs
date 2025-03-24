using System.Collections.Concurrent;
using System.Numerics;

public class RoomActor
{ 
    public int ActorId { get; private set; }
    public Vector3 Pos { get; private set; }
    public Vector3 Rot { get; private set; }

    public void Init(int actorId, Vector3 pos, Vector3 rot)
    {
        ActorId = actorId;
        Pos = pos;
        Rot = rot;
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

    public int AddActor(Vector3 pos, Vector3 rot)
    {
        RoomActor actor = new RoomActor();
        int actorId = GenerateActorId();
        actor.Init(actorId, pos, rot);
        Actors.TryAdd(actorId, actor);
        return actorId;
    }

    public int GenerateActorId()
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
                    return -1;
                }
            }
        }
        return generateActorId;
    }
}