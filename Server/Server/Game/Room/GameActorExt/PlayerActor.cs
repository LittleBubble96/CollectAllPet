
using ShareProtobuf.ShareData;

public class PlayerActor : RoomActor
{
    private DateTime m_lastAddGoldTime;
    private int m_lastAddGold;
    public override void OnInit()
    {
        base.OnInit();
        //添加属性
        AddAttribute((int)EPlayerAttribute.Gold, 0);
        AddAttribute((int)EPlayerAttribute.Diamond, 0);
        AddAttribute((int)EPlayerAttribute.DeltaGold, 0);
        m_lastAddGoldTime = DateTime.Now;
    }
    
    // 金币增加
    public void AddGold(int gold)
    {
        if (gold <= 0)
        {
            return;
        }
        int curGold = GetIntAttribute((int)EPlayerAttribute.Gold);
        UpdateAttribute((int)EPlayerAttribute.Gold, curGold + gold);
        m_lastAddGold += gold;
        
        float millSeconds = (float)(DateTime.Now - m_lastAddGoldTime).TotalMilliseconds;
        if (millSeconds > 1000)
        {
            float deltaGold = (int)(m_lastAddGold / millSeconds) * 1000;
            UpdateAttribute((int)EPlayerAttribute.DeltaGold, deltaGold);
        }
    }
    
    //钻石增加
    public void AddDiamond(int diamond)
    {
        if (diamond <= 0)
        {
            return;
        }
        int curDiamond = GetIntAttribute((int)EPlayerAttribute.Diamond);
        UpdateAttribute((int)EPlayerAttribute.Diamond, curDiamond + diamond);
    }
    
}