public class LoginGameState : GameStateBase
{
    public override void OnEnter()
    {
        base.OnEnter();
        GameManager.GetUIManager().ShowUI<Login_UI>();
    }
    
    public override void OnExit()
    {
        base.OnExit();
    }
}