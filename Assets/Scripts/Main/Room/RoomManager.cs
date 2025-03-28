using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ShareProtobuf;
using ShareProtobuf.ShareData;
using UnityEngine;


public class RoomManager : Singleton<RoomManager>
{
    private struct RoomActorTempInfo
    {
        public string ResName;
        public string Name;
    }

    
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
                    //开始同步
                    GameManager.GetGameSyncActorManager().StartSync();
                    break;
                case ERoomState.End:
                    //结束同步
                    GameManager.GetGameSyncActorManager().StopSync();
                    break;
            }
        }
    }

    public void Init()
    {
        // Init RoomManager
    }

    public void DoFixedUpdate()
    {
        // Do FixedUpdate
        foreach (var actor in actorDict)
        {
            actor.Value.DoFixedUpdate();
        }
    }

    // Update Room Detail refActorId is Host
    public void UpdateDetailRoom(RoomDetailInfo roomDetail)
    {
        this.roomDetailInfo = roomDetail;
    }
    
    public void UpdateHostActorId(int inRefActorId)
    {
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
        if (roomDetailInfo != null && roomDetailInfo.WorldActors != null)
        {
            int actorCount = roomDetailInfo.WorldActors.Count;
            int curActorIndex = 0;
            //加载玩家
            foreach (var actorInfo in roomDetailInfo.WorldActors)
            {
                curActorIndex++;
                progressCallback?.Invoke(curActorIndex / (float)actorCount);
                CreateRoomActor(actorInfo);
                yield return null;
            }
        }
        yield return null;
    }

    public void CreateRoomActor(GameActorInfo actorInfo)
    {
        if (actorDict.ContainsKey(actorInfo.RefActorId))
        {
            return;
        }
        //加载Actor
        RoomActorTempInfo tempInfo = GetRoomActorTempInfo(actorInfo);
        if (tempInfo.ResName == null || tempInfo.ResName == "")
        {
            return;
        }
        GameObject go = Resources.Load<GameObject>(tempInfo.ResName);
        if (go != null)
        {
            GameObject actor = GameObject.Instantiate(go);
            actor.name = actorInfo.ActorName;
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
    }
    
    private RoomActorTempInfo GetRoomActorTempInfo(GameActorInfo actorInfo)
    {
        RoomActorTempInfo tempInfo = new RoomActorTempInfo();
        if (actorInfo.ActorRoleType == (int)EActorRoleType.Player)
        {
            PlayerConfigItem playerConfigItem = PlayerConfig.GetPlayerConfigItem(actorInfo.ActorConfigId);
            if (playerConfigItem != null)
            {
                tempInfo.ResName = playerConfigItem.Prefab;
                tempInfo.Name = playerConfigItem.Name;
            }
        }
        else if (actorInfo.ActorRoleType == (int)EActorRoleType.Monster)
        {
            MonsterConfigItem monsterConfigItem = MonsterConfig.GetConfigItem(actorInfo.ActorConfigId);
            if (monsterConfigItem != null)
            {
                tempInfo.ResName = monsterConfigItem.Prefab;
                tempInfo.Name = monsterConfigItem.Name;
            }
        }
      
        return tempInfo;
    }
 

    //同步服务器Actor信息
    public void SyncServerActorInfo(List<DeltaActorSyncData> deltaActorSyncData)
    {
        foreach (var syncData in deltaActorSyncData)
        {
            if (actorDict.TryGetValue(syncData.ActorId, out Actor actor))
            {
                actor.SetServerPosition(ConfigHelper.ConvertVector3ToUnityVector3(syncData.Pos));
                actor.SetServerRotation(ConfigHelper.ConvertVector3ToUnityVector3(syncData.Rot));
                actor.SetServerSpeed(ConfigHelper.ConvertVector3ToUnityVector3(syncData.Speed));
                actor.SetActorState(EActorState.Ready);
            }
        }
    }

    #endregion
}