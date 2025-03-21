public class MainGameState : GameStateBase
{
    public override void OnEnter()
    {
        base.OnEnter();
        GameManager.GetUIManager().ShowUI<RoomMain_UI>();
    }
}