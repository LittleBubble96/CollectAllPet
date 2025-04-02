public class BTPetHatedTN : BTTaskNode
{
    protected override void OnRecycle()
    {
        
    }

    protected override void OnBegin()
    {
        TargetComponent targetComponent = behaviorTree.GetAIController().TryOrAddActorComponent<TargetComponent>();
        targetComponent.SetTargeting(true);
        
        //发送消息
        ClientRequestFunc.SendFindPetTargetRequest(behaviorTree.GetAIController().GetActorId(), targetComponent.TargetActorId);
    }

    protected override void OnEnd()
    {
        
    }

    protected override BtNodeResult OnExecute(float deltaTime)
    {
        TargetComponent targetComponent = behaviorTree.GetAIController().GetActorComponent<TargetComponent>();
        if (targetComponent.IsTargeting)
        {
            return BtNodeResult.InProgress;
        }

        return targetComponent.TargetIsValid() ? BtNodeResult.Succeeded : BtNodeResult.Failed;
    }

    protected override void OnParseParams(string[] args)
    {
        
    }
}