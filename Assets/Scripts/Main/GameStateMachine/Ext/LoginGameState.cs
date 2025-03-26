using System.Collections;

public class LoginGameState : GameStateBase
{
    public override IEnumerator OnEnterAsync()
    {
        GameManager.GetUIManager().ShowUI<Login_UI>();
        yield return null;
    }
}