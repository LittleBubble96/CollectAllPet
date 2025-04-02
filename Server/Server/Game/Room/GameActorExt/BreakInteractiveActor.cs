
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
    }
}