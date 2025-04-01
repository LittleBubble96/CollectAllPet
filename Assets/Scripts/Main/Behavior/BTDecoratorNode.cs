public abstract class BTDecoratorNode : BehaviorNode
{
    protected bool DecoratorResult = false;

    public override void Recycle()
    {
        base.Recycle();
        DecoratorResult = false;
        OnRecycle();
    }

    public override void Begin()
    {
        base.Begin();
        OnBegin();
    }
    
    public override void Execute(float deltaTime)
    {
        base.Execute(deltaTime);
        BTDecoratorExecuteResult result = OnExecute(deltaTime);
        if (result != null)
        {
            this.DecoratorResult = result.ExecuteResult;
        }
        else 
        {
            this.DecoratorResult = false;
        }

        if (this.cfgBTItem.Inversed)
        {
            this.DecoratorResult = !this.DecoratorResult;
        }
    }

    public override bool GetCanExecuteResult()
    {
        return DecoratorResult;
    }

    protected override void ParseParams(string[] args)
    {
        base.ParseParams(args);
        if (args != null && args.Length > 0)
        {
            OnParseParams(args);
        }
    }

    public override void End()
    {
        base.End();
        OnEnd();
    }

    protected abstract void OnRecycle();
    protected abstract void OnBegin();
    protected abstract void OnEnd();
    protected abstract BTDecoratorExecuteResult OnExecute(float deltaTime);
    protected abstract void OnParseParams(string[] args);
}

/**
 * 装饰器OnExecute执行结果
 */
public class BTDecoratorExecuteResult
{
    //是否满足执行前的条件检测
    public bool ExecuteResult= false;
}