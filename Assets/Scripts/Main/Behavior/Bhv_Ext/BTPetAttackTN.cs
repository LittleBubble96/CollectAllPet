public class BTPetAttackTN : BTTaskNode
{
    private float attackInterval = 0;
    private float attackAnimTime = 0;
    protected override void OnRecycle()
    {
        attackInterval = 0;
        attackAnimTime = 0;
    }

    protected override void OnBegin()
    {
        attackInterval = behaviorTree.GetAIController().GetAttackInterval();
        //播放攻击动画
        attackAnimTime = behaviorTree.GetAIController().GetAttackAnimDuration();
    }

    protected override void OnEnd()
    {
        
    }

    protected override BtNodeResult OnExecute(float deltaTime)
    {
        if (attackInterval > 0)
        {
            attackInterval -= deltaTime;
            if (attackAnimTime > 0)
            {
                attackAnimTime -= deltaTime;
                if (attackAnimTime <= 0)
                {
                    behaviorTree.GetAIController().GetAnimationController().SetBool("Attack",false);
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