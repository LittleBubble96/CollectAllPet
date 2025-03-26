using System.Collections;

public class StateBase
{
    public virtual IEnumerator OnEnterAsync()
    {
        yield break;
    }
    public virtual IEnumerator OnExitAsync()
    {
        yield break;
    }
    public virtual void DoUpdate(float dt)
    {
        
    }
}