
using ShareProtobuf;
using ShareProtobuf.ShareData;

public class BreakInteractiveActor : RoomActor
{
    private BreakInteractiveItem breakInteractiveItem;
    
    public override void OnInit()
    {
        base.OnInit();
        breakInteractiveItem = BreakInteractiveConfig.GetConfigItem(ActorCfgId);
        if (breakInteractiveItem == null)
        {
            Console.WriteLine("break interactive item is null");
            return;
        }
        AddAttribute((int)EBreakInteractiveAttribute.Hp, breakInteractiveItem.HP);
    }

    public void Damage(int atkId , int ackValue)
    {
        Hp -= ackValue;
        if (Hp < 0)
        {
            Destroy();
        }
    }

    public int Hp
    {
        get 
        {
            return GetIntAttribute((int)EBreakInteractiveAttribute.Hp);
        }
        set 
        {
            UpdateAttribute((int)EBreakInteractiveAttribute.Hp, value);
        }
    }

    public string DestroyEffectName
    {
        get 
        {
            return breakInteractiveItem.DestroyEffectName;
        }
    }
    
    public Vector3 DestroyEffectOffset
    {
        get 
        {
            return breakInteractiveItem.DestroyEffectOffset;
        }
    }
    
    public string HitEffectName
    {
        get 
        {
            return breakInteractiveItem.HitEffectName;
        }
    }
    
    public Vector3 HitEffectOffset
    {
        get 
        {
            return breakInteractiveItem.HitEffectOffset;
        }
    }
}