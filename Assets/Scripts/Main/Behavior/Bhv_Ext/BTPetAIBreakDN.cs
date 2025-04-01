public class BTPetAIBreakDN : BTDecoratorNode
{
    private BTDecoratorExecuteResult m_ExecuteResult = new BTDecoratorExecuteResult();
    protected override void OnRecycle()
    {
        
    }

    protected override void OnBegin()
    {
        
    }

    protected override void OnEnd()
    {
        
    }

    protected override void OnParseParams(string[] args)
    {
        
    }

    protected override BTDecoratorExecuteResult OnExecute(float deltaTime)
    {
        SetSuccess();
        return m_ExecuteResult;
    }
    
    protected void SetSuccess()
    {
        m_ExecuteResult.ExecuteResult = true;
    }
    
    protected void SetFailed()
    {
        m_ExecuteResult.ExecuteResult = false;
    }
}