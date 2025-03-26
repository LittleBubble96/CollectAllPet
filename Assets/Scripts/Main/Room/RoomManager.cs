using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ShareProtobuf;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    private int enterRoomId = -1;
    private ERoomState roomState = ERoomState.None;
    private RoomDetailInfo roomDetailInfo;
    private int refActorId;
    private ConcurrentDictionary<int, Actor> actorDict = new ConcurrentDictionary<int, Actor>();
    private ConcurrentDictionary<int,Actor> ownerActorDict = new ConcurrentDictionary<int, Actor>();

    public ERoomState RoomState
    {
        get { return roomState; }
        set
        {
            if (roomState == value) return;
            roomState = value;
            switch (roomState)
            {
                case ERoomState.None:
                    break;
                case ERoomState.Waiting:
                    //发送消息
                    OnWaitRoom();
                    break;
                case ERoomState.Loading:
                    break;
                case ERoomState.Playing:
                    break;
                case ERoomState.End:
                    break;
            }
        }
    }

    public void Init()
    {
        // Init RoomManager
    }
    
    // Update Room Detail refActorId is Host
    public void UpdateDetailRoom(RoomDetailInfo roomDetail,int inRefActorId)
    {
        this.roomDetailInfo = roomDetail;
        this.refActorId = inRefActorId;
    }
    
    public RoomDetailInfo GetRoomDetailInfo()
    {
        return roomDetailInfo;
    }
    
    public int GetRefActorId()
    {
        return refActorId;
    }
    
    public void EnterRoom(int roomId)
    {
        // Enter Room
        enterRoomId = roomId;
    }
    
    public int GetEnterRoomId()
    {
        return enterRoomId;
    }
    
    public void CreateRoom(string roomName ,int maxPlayerCount)
    {
        // Create Room
        ClientRequestFunc.SendCreateRoomRequest(roomName,maxPlayerCount);
    }
    
    public void JoinRoom(int roomId)
    {
        // Join Room
        ClientRequestFunc.SendJoinRoomRequest(roomId);
    }
    
    public void RefreshRoomList()
    {
        // Refresh Room List
        ClientRequestFunc.RefreshRoomListRequest();
    }
    
    public ConcurrentDictionary<int,Actor> GetActorDict()
    {
        return actorDict;
    }
    
    //获取属于自己的Actor 集合
    public ConcurrentDictionary<int,Actor> GetOwnerActorDict()
    {
        return ownerActorDict;
    }
    

    #region 房间世界
    public void OnWaitRoom()
    {
        // Wait Room
        //开启协程
        ClientRequestFunc.GetRoomDetailRequest(enterRoomId,CharacterManager.Instance.PlayerInfo.PlayerId);
    }
    
    public void SpawnRoomWorld()
    {
        // Spawn Room World
    }
    
    public void DestroyRoomWorld()
    {
        // Destroy Room World
        foreach (var actor in actorDict)
        {
            GameObject.Destroy(actor.Value.gameObject);
        }
        actorDict.Clear();
        ownerActorDict.Clear();
    }
    
    public IEnumerator LoadSceneActor(Action<float> progressCallback)
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
                progressCallback?.Invoke(curActorIndex / (float)actorCount);
                if (actorDict.ContainsKey(actorInfo.RefActorId))
                {
                    continue;
                }
                //加载Actor
                GameObject go = Resources.Load<GameObject>(actorInfo.ActorRes);
                if (go != null)
                {
                    GameObject actor = GameObject.Instantiate(go);
                    Actor actorCmpt = actor.GetComponent<Actor>();
                    if (actorCmpt != null)
                    {
                        actorCmpt.InitActor(actorInfo);
                        actorDict.TryAdd(actorInfo.RefActorId, actorCmpt);
                        if (actorCmpt.IsOwnerPlayer())
                        {
                            ownerActorDict.TryAdd(actorInfo.RefActorId, actorCmpt);
                        }
                    }

                    actor.transform.position = GameConst.DefaultActorPosition;
                    actor.transform.rotation = GameConst.DefaultActorRotation;
                }
                yield return null;
            }
        }
        yield return null;
    }

    #endregion
}