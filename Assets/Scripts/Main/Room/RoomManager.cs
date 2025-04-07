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
    private RoomSceneController roomSceneController;
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
                    roomSceneController.Init();
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
        roomSceneController = new RoomSceneController();
    }

    public void DoFixedUpdate()
    {
        // Do FixedUpdate
        foreach (var actor in actorDict)
        {
            actor.Value.DoFixedUpdate();
        }
    }
    
    public void DoUpdate(float deltaTime)
    {
        // Do Update
    }

    // Update Room Detail refActorId is Host
    public void UpdateDetailRoom(RoomDetailInfo roomDetail)
    {
        this.roomDetailInfo = roomDetail;
    }
    
    public void UpdateHostActorId(int inRefActorId)
    {
        this.refActorId = inRefActorId;
        if (actorDict.TryGetValue(refActorId, out Actor actor))
        {
            roomSceneController.SetCameraLookAt(actor);
        }
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
    
    public Actor GetActor(int actorId)
    {
        if (actorDict.TryGetValue(actorId, out Actor actor))
        {
            return actor;
        }
        return null;
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
        RecycleObject actor = GOtPoolManager.Instance.Get<RecycleObject>(tempInfo.ResName);
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
        CharacterController characterController = actor.GetComponent<CharacterController>();
        if (characterController)
        {
            characterController.Move( ConfigHelper.ConvertVector3ToUnityVector3(actorInfo.SpawnPos));
        }
        else
        {
            actor.transform.position = ConfigHelper.ConvertVector3ToUnityVector3(actorInfo.SpawnPos);
        }
        actor.transform.rotation = Quaternion.Euler( ConfigHelper.ConvertVector3ToUnityVector3(actorInfo.SpawnRot));
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
        else if (actorInfo.ActorRoleType == (int)EActorRoleType.BreakInteractive)
        {
            BreakInteractiveItem breakInteractiveItem = BreakInteractiveConfig.GetConfigItem(actorInfo.ActorConfigId);
            if (breakInteractiveItem != null)
            {
                tempInfo.ResName = breakInteractiveItem.Prefab;
                tempInfo.Name = breakInteractiveItem.Name;
            }
        }
      
        else
        {
            Debug.LogError("Actor Role Type Error");
        }
      
        return tempInfo;
    }
    
    //销毁Actor
    public void DestroyActor(int actorId)
    {
        if (actorDict.TryRemove(actorId, out Actor actor))
        {
            GOtPoolManager.Instance.Return(actor);
        }
        ownerActorDict.TryRemove(actorId, out Actor ownerActor);
    }
 

    //同步服务器Actor信息
    public void SyncServerActorInfo(List<DeltaActorSyncData> deltaActorSyncData)
    {
        if (deltaActorSyncData == null)
        {
            return;
        }
        foreach (var syncData in deltaActorSyncData)
        {
            if (actorDict.TryGetValue(syncData.ActorId, out Actor actor))
            {
                actor.SetServerPosition(ConfigHelper.ConvertVector3ToUnityVector3(syncData.Pos));
                actor.SetServerRotation(ConfigHelper.ConvertVector3ToUnityVector3(syncData.Rot));
                actor.SetServerSpeed(ConfigHelper.ConvertVector3ToUnityVector3(syncData.Speed));
                actor.SetServerProperties(syncData.UpdateAttribute);
                actor.SetActorState(EActorState.Ready);
            }
        }
    }
    
    //同步服务器Actor动画信息
    public void SyncServerActorAnimationInfo(List<DeltaActorAnimationSyncData> deltaActorAnimationSyncData)
    {
        foreach (var syncData in deltaActorAnimationSyncData)
        {
            if (actorDict.TryGetValue(syncData.ActorId, out Actor actor))
            {
                actor.SetServerAnimationParams(syncData);
            }
        }
    }
    
    //同步服务器 actor属性信息
    public void SyncServerActorPropertiesInfo(List<int> actorIds, List<string> propJsons)
    {
        for (int i = 0; i < actorIds.Count; i++)
        {
            if (actorDict.TryGetValue(actorIds[i], out Actor actor))
            {
                actor.SetServerProperties(propJsons[i]);
            }
        }
    }

    #endregion

    #region 宠物逻辑

    public void SetActorTarget(int petActorId, int targetActorId)
    {
        if (actorDict.TryGetValue(petActorId, out Actor petActor))
        {
            TargetComponent target = petActor.GetActorComponent<TargetComponent>();
            if (target != null)
            {
                target.SetTargetActorId(targetActorId);
                target.SetTargeting(false);
            }
        }
    }

    #endregion
}