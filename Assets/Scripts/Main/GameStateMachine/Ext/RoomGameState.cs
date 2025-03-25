using System.Collections;

public class RoomGameState : GameStateBase
{
    public override void OnEnter()
    {
        base.OnEnter();
        GameManager.GetUIManager().ShowUI<RoomGame_UI>();
    }
    
    private IEnumerator OnEnterAsync()
    {
        //更新房间详细信息
        GameManager.GetUIManager().ShowLockUI();
        RoomManager.Instance.RoomState = ERoomState.Waiting;
        
        yield return null;
    }
}