using System.Collections;
using ShareProtobuf;
using UnityEngine;

public class RoomGameState : GameStateBase
{
    public override IEnumerator OnEnterAsync()
    {
        GameManager.GetUIManager().ShowUI<RoomGame_UI>();
        yield return OnEnterAsync_Internal();
    }
    
    private IEnumerator OnEnterAsync_Internal()
    {
        //更新房间详细信息
        GameManager.GetUIManager().ShowLockUI();
        RoomManager.Instance.RoomState = ERoomState.Waiting;
        yield return new WaitUntil(() => RoomManager.Instance.RoomState == ERoomState.Loading);
        GameManager.GetAppEventDispatcher().BroadcastListener(EventName.EVENT_LoadingUIProcess, 0.3f);
        //加载场景 0.3 - 0.6
        yield return CAP_SceneManager.Instance.LoadScene("RoomScene",null, (progress) =>
        {
            GameManager.GetAppEventDispatcher().BroadcastListener(EventName.EVENT_LoadingUIProcess, 0.3f + progress * 0.3f);
        });
        //加载场景物体
        yield return RoomManager.Instance.LoadSceneActor((process) =>
        {
            GameManager.GetAppEventDispatcher().BroadcastListener(EventName.EVENT_LoadingUIProcess, 0.6f + 0.3f * process);
        });
        //通知创建 当前角色
        ClientRequestFunc.SendCreatePlayerActorRequest();
        RoomManager.Instance.RoomState = ERoomState.Playing;
    }
    
}