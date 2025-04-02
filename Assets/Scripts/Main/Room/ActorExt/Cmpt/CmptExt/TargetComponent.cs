public class TargetComponent : ActorComponent
{
    public bool IsTargeting { get; private set; }
    public int TargetActorId { get; private set; }
    public int LastTargetActorId { get; private set; }

    public void SetTargeting(bool isTargeting)
    {
        IsTargeting = isTargeting;
    }
    
    public void SetTargetActorId(int actorId)
    {
        LastTargetActorId = TargetActorId;
        TargetActorId = actorId;
    }

    public bool TargetIsValid()
    {
        return TargetActorId <= 0;
    }

}