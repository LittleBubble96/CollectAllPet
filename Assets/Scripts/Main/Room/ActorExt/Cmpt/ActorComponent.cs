public class ActorComponent : IRecycle
{
    protected Actor actor;
    public void Init(Actor actor)
    {
        this.actor = actor;
    }

    public void Recycle()
    {
        actor = null;
    }
}