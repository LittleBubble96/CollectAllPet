
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
    
    public string HitEffectName
    {
        get 
        {
            return breakInteractiveItem.HitEffectName;
        }
    }
    
    //获取破坏后随机金币
    public int GetRandomCoin()
    {
        int random = RandomHelper.GetRandom(breakInteractiveItem.GenGoldRandoms[0],breakInteractiveItem.GenGoldRandoms[1]);
        
        return random;
    }
    
    //获取破坏后随机钻石
    public int GetRandomDiamond()
    {
        float randomRate = RandomHelper.GetRandom(0, 1f);
        if (randomRate < breakInteractiveItem.GenDiamondProbability)
        {
            int random = RandomHelper.GetRandom(breakInteractiveItem.GenDiamondRandoms[0],breakInteractiveItem.GenDiamondRandoms[1]);
            return random;
        }
        
        return 0;
    }
}