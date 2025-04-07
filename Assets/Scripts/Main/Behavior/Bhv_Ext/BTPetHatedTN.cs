using UnityEngine;

public class BTPetHatedTN : BTTaskNode
{
    //如果找不到的时间间隔 
    private float findTargetInterval = 1f;
    private float findTargetTime = 0;
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

        findTargetTime = targetComponent.TargetIsValid() ? 0 : findTargetTime + deltaTime;
        if (findTargetTime > 0)
        {
            findTargetTime -= deltaTime;
            return BtNodeResult.InProgress;
        }
        findTargetTime = 0;
        return targetComponent.TargetIsValid() ? BtNodeResult.Succeeded : BtNodeResult.Failed;
    }

    protected override void OnParseParams(string[] args)
    {
        
    }
}