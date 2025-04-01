using ShareProtobuf.ShareData;
using UnityEngine;

public class AIController : Actor
{
    //行为树
    protected BehaviorTree behaviorTree;
    protected override void OnInit()
    {
        base.OnInit();
        behaviorTree = new BehaviorTree();
        behaviorTree.Init(new BTGenInfo(GetAiId()));
    }

    protected override void DirectUpdate()
    {
        base.DirectUpdate();
        if (behaviorTree != null)
        {
            behaviorTree.Execute(Time.fixedDeltaTime);
        }
    }

    protected int GetAiId()
    {
        //TODO 目前就是只有宠物用按这个脚本所以获取宠物配置
        MonsterConfigItem cfg = MonsterConfig.GetConfigItem(this.actorInfo.ActorConfigId);
        if (cfg == null)
        {
            return -1;
        }
        return cfg.AiId;
    }
}