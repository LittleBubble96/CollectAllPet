public class BTPetPreBhvTN : BTTaskNode
{
    private bool findSucc = false;
    protected override void OnRecycle()
    {
        findSucc = false;
    }

    protected override void OnBegin()
    {
        //获取目标
        TargetComponent targetComponent = behaviorTree.GetAIController().TryOrAddActorComponent<TargetComponent>();
        if (targetComponent == null)
        {
            findSucc = false;
            return;
        }

        if (targetComponent.TargetActorId<=0)
        {
            findSucc = false;
            return;
        }
        Actor actor = RoomManager.Instance.GetActor(targetComponent.TargetActorId);
        if (actor== null)
        {
            findSucc = false;
            return;
        }
        findSucc = true;
        
        behaviorTree.GetAIController().SetTargetPosition(actor.GetTfPosition());
    }

    protected override void OnEnd()
    {
        
    }

    protected override BtNodeResult OnExecute(float deltaTime)
    {
        if (!findSucc)
        {
            return BtNodeResult.Failed;
        }
        TargetComponent targetComponent = behaviorTree.GetAIController().GetActorComponent<TargetComponent>();
        if (targetComponent == null)
        {
            return BtNodeResult.Failed;
        }
        if (targetComponent.TargetActorId<=0)
        {
            return BtNodeResult.Failed;
        }
        Actor actor = RoomManager.Instance.GetActor(targetComponent.TargetActorId);
        if (actor== null)
        {
            return BtNodeResult.Failed;
        }
        //
        if (behaviorTree.GetAIController().IsAgentStopped())
        {
            return BtNodeResult.Succeeded;
        }

        return BtNodeResult.Failed;
    }

    protected override void OnParseParams(string[] args)
    {
        
    }
}