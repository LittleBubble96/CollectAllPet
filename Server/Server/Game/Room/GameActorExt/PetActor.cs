
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
            return;
        }
        //添加属性
        AddAttribute((int)EMonsterAttribute.AttackRange, petConfigItem.AttackRange);
        AddAttribute((int)EMonsterAttribute.CancelAttackRange, petConfigItem.CancelAttackRange);
        AddAttribute((int)EMonsterAttribute.AttackInterval, petConfigItem.AttackInterval);
        AddAttribute((int)EMonsterAttribute.AttackDamage, petConfigItem.AttackDamage);
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