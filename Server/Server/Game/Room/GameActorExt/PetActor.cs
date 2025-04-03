
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
        return GetFloatAttribute((int)EMonsterAttribute.AttackRange);
    }
    
    public float GetCancelAttackRange()
    {
        return GetFloatAttribute((int)EMonsterAttribute.CancelAttackRange);
    }
    public float GetAttackInterval()
    {
        return GetFloatAttribute((int)EMonsterAttribute.AttackInterval);
    }
    public int GetAttackDamage()
    {
        return GetIntAttribute((int)EMonsterAttribute.AttackDamage);
    }
}