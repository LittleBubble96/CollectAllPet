using UnityEngine;

public class BTPetAttackTN : BTTaskNode
{
    private float attackInterval = 0;
    private float attackAnimTime = 0;
    private float attackHitTime = 0;
    protected override void OnRecycle()
    {
        attackInterval = 0;
        attackAnimTime = 0;
        attackHitTime = 0;
    }

    protected override void OnBegin()
    {
        attackInterval = behaviorTree.GetAIController().GetAttackInterval();
        //播放攻击动画
        attackAnimTime = behaviorTree.GetAIController().GetAttackAnimDuration();
        attackHitTime = behaviorTree.GetAIController().GetAttackHitTime();
        behaviorTree.GetAIController().GetAnimationController().SetBool("Attack",true);
        Debug.Log("[AI] Set Attack Animation: " + attackAnimTime + "attackInterval: " + attackInterval);
    }

    protected override void OnEnd()
    {
        
    }

    protected override BtNodeResult OnExecute(float deltaTime)
    {
        if (attackInterval > 0)
        {
            attackInterval -= deltaTime;
            //攻击间隔未到
            if (attackAnimTime > 0)
            {
                attackAnimTime -= deltaTime;
                if (attackAnimTime <= 0)
                {
                    behaviorTree.GetAIController().GetAnimationController().SetBool("Attack",false);
                }
            }
            //打击时间未到
            if (attackHitTime > 0)
            {
                attackHitTime -= deltaTime;
                if (attackHitTime <= 0)
                {
                    TargetComponent target = behaviorTree.GetAIController().GetActorComponent<TargetComponent>();
                    if (target != null)
                    {
                        ClientRequestFunc.SendPetAttackRequest(behaviorTree.GetAIController().GetActorId(),target.TargetActorId);
                    }
                }
            }
            return BtNodeResult.InProgress;
        }
        behaviorTree.GetAIController().GetAnimationController().SetBool("Attack",false);
        return BtNodeResult.Succeeded;
    }

    protected override void OnParseParams(string[] args)
    {
        
    }
}