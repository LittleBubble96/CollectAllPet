using System.Collections.Generic;
using UnityEngine;

public class BTSelectorNode : BehaviorNode
{
    private BtNodeResult BtNodeRunRes;
    private int CurrentIndex = 0;
    
    private List<BehaviorNode> Children = new List<BehaviorNode>();
    private List<BehaviorNode> Services = new List<BehaviorNode>();
    private List<BehaviorNode> Decorators = new List<BehaviorNode>();
    
    public override void AssembleBTNode(BehaviorTree bt, ConfBTCShape cfg)
    {
        base.AssembleBTNode(bt, cfg);
        this.Decorators = Assemble(bt, cfg.Decorators);
        this.Services = Assemble(bt, cfg.Services);
        this.Children = Assemble(bt, cfg.Children);
    }

    public override void Recycle()
    {
        base.Recycle();
        foreach (var child in Children)
        {
            ClientFactory.Instance.GetBehaviorNodeFactory().PutObject(child);
        }
        foreach (var service in Services)
        {
            ClientFactory.Instance.GetBehaviorNodeFactory().PutObject(service);
        }
        foreach (var decorator in Decorators)
        {
            ClientFactory.Instance.GetBehaviorNodeFactory().PutObject(decorator);
        }
        Children.Clear();
        Services.Clear();
        Decorators.Clear();
        this.BtNodeRunRes = BtNodeResult.None;
        this.CurrentIndex = 0;
    }

    public override void Begin()
    {
        base.Begin();
        foreach (var decorator in Decorators)
        {
            decorator.Begin();
        }
        BtNodeRunRes = BtNodeResult.None;
    }
    
    public override void End()
    {
        base.End();
        foreach (var decorator in Decorators)
        {
            decorator.End();
        }
        foreach (var service in Services)
        {
            service.End();
        }

        if (this.CurrentIndex >= 0 && this.CurrentIndex < Children.Count)
        {
            BehaviorNode child = Children[this.CurrentIndex];
            child.End();
        }
    }
    public override void Execute(float deltaTime)
    {
        base.Execute(deltaTime);
        foreach (var decorator in Decorators)
        {
            decorator.Execute(deltaTime);
            //如果装饰器返回失败，则直接返回
            if (!decorator.GetCanExecuteResult())
            {
                this.BtNodeRunRes = BtNodeResult.Failed;
                return;
            }
        }

        this.BtNodeRunRes = OnExecute(deltaTime);
    }

    protected BtNodeResult OnExecute(float deltaTime)
    {
        int index = this.CurrentIndex;
        BtNodeResult lastResult = this.BtNodeRunRes;
        if (lastResult != BtNodeResult.InProgress)
        {
            index = 0;
        }
        for (int i = index; i < Children.Count; i++)
        {
            BehaviorNode child = Children[i];
            if (lastResult != BtNodeResult.InProgress)
            {
                child.Begin();
            }
            child.Execute(deltaTime);
            BtNodeResult runRes = child.RunState();
            switch (runRes)
            {
                case BtNodeResult.Succeeded:
                    child.End();
                    return BtNodeResult.Succeeded;
                case BtNodeResult.Failed:
                    child.End();
                    break;
                case BtNodeResult.InProgress:
                    this.CurrentIndex = i;
                    return BtNodeResult.InProgress;
                default:
                    Debug.LogError( $"BTSelectorNode OnExecute: runRes is error, runRes: {runRes}");
                    break;
            }
        }
        return BtNodeResult.Failed;
    }

    public override BtNodeResult RunState()
    {
        return BtNodeRunRes;
    }
}