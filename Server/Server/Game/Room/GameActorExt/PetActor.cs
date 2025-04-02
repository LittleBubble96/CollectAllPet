
using ShareProtobuf.ShareData;

public class PetActor : RoomActor
{
    private MonsterConfigItem petConfigItem;
    public override void OnInit()
    {
        base.OnInit();
        petConfigItem = MonsterConfig.GetConfigItem(ActorCfgId);
        if (petConfigItem == null)
        {
            Console.WriteLine($"[error]宠物配置不存在, ActorCfgId:{ActorCfgId}");
        }
    }

    public float GetAttackRange()
    {
        if (petConfigItem == null)
        {
            return 0;
        }
        return petConfigItem.AttackRange;
    }
    
    public float GetCancelAttackRange()
    {
        if (petConfigItem == null)
        {
            return 0;
        }
        return petConfigItem.CancelAttackRange;
    }
}