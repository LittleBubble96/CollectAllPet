using System.Collections;

public enum GameStateEnum
{
    Login,
    Main,
    RoomGame,
}

public class GameStateMachine : StateMachineBase
{
    private bool isLoading;
    public bool IsLoading
    {
        get
        {
            return isLoading;
        }
        set
        {
            isLoading = value;
            if (isLoading)
            {
                GameManager.GetUIManager().ShowLoadingUI();
            }
            else
            {
                // Hide loading UI
                //GameManager.GetUIManager().HideLoadingUI();
            }
        }
    }
    
    public override void Init()
    {
        base.Init();
        ChangeGameState(GameStateEnum.Login);
    }
    
    public void ChangeGameState(GameStateEnum state)
    {
        GameManager.Instance.StartCoroutine(ChangeGameStateEnumerator(state));
    }
    
    private IEnumerator ChangeGameStateEnumerator(GameStateEnum state)
    {
        if (CurrentState != null)
        {
            IsLoading = true;
            yield return CurrentState.OnExitAsync();
            if (PreviousState != null)
            {
                PutState(PreviousState as GameStateBase);
            }
            PreviousState = CurrentState;
        }
        //规定卸载之前状态进度为 0.2
        GameManager.GetAppEventDispatcher().BroadcastListener(EventName.EVENT_LoadingUIProcess, 0.2f);
        CurrentState = GenerateState(state);
        yield return CurrentState.OnEnterAsync();
        GameManager.GetAppEventDispatcher().BroadcastListener(EventName.EVENT_LoadingUIProcess, 1f);
        if (IsLoading)
        {
            isLoading = false;
        }
    }
    
    public GameStateBase GenerateState(GameStateEnum state)
    {
        return ClientFactory.Instance.GetGameStateFactory().GetObject(state);
    }
    
    public void PutState(GameStateBase state)
    {
        ClientFactory.Instance.GetGameStateFactory().PutObject(state);
    }
}