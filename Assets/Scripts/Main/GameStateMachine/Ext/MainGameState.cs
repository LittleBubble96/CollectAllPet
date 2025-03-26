using System.Collections;

public class MainGameState : GameStateBase
{
    public override IEnumerator OnEnterAsync()
    {
        GameManager.GetUIManager().ShowUI<RoomMain_UI>();
        yield return null;
    }
    
    public override IEnumerator OnExitAsync()
    {
        GameManager.GetUIManager().HideMainUI();
        yield return null;
    }
}