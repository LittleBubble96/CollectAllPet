using System.Collections;
using ShareProtobuf;
using UnityEngine;

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
        yield return new WaitUntil(() => RoomManager.Instance.RoomState == ERoomState.Loading);
        GameManager.GetAppEventDispatcher().BroadcastListener(EventName.EVENT_LoadingUIProcess, 0.3f);
        //加载场景 0.3 - 0.6
        yield return CAP_SceneManager.Instance.LoadScene("RoomScene",null, (progress) =>
        {
            GameManager.GetAppEventDispatcher().BroadcastListener(EventName.EVENT_LoadingUIProcess, 0.3f + progress * 0.3f);
        });
        //加载场景物体
        yield return null;
    }
    
    private IEnumerator LoadSceneActor()
    {
        RoomDetailInfo roomDetailInfo = RoomManager.Instance.GetRoomDetailInfo();
        if (roomDetailInfo != null)
        {
            int actorCount = roomDetailInfo.WorldActors.Count;
            int curActorIndex = 0;
            //加载玩家
            foreach (var actorInfo in roomDetailInfo.WorldActors)
            {
                curActorIndex++;
                GameManager.GetAppEventDispatcher().BroadcastListener(EventName.EVENT_LoadingUIProcess, 0.6f + 0.3f * curActorIndex / actorCount);
                //加载Actor
                GameObject go = Resources.Load<GameObject>(actorInfo.ActorRes);
                if (go != null)
                {
                    GameObject actor = GameObject.Instantiate(go);
                    // actor.transform.position = actorInfo.;
                    // actor.transform.rotation = actorInfo.ActorRot;
                    // actor.transform.localScale = actorInfo.ActorScale;
                    // actor.name = actorInfo.ActorName;
                    //加载Actor
                    yield return null;
                }
                yield return null;
            }
        }
        yield return null;
    }
}