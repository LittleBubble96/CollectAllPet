public class StateMachineBase
{
    protected StateBase CurrentState;
    protected StateBase PreviousState;

    public virtual void Init()
    {

    }

    public virtual void DoUpdate(float dt)
    {
        if (CurrentState != null)
        {
            CurrentState.DoUpdate(dt);
        }
    }

    public virtual void Dispose()
    {
        
    }

}