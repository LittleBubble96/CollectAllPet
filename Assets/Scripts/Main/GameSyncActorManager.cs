using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ShareProtobuf;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class GameSyncActorManager : MonoBehaviour
{
    //网络同步信息
//     检查同步逻辑放在 FixedUpdate中，固定步长（0.02s）
//     高优先级：（100ms 一次）即一秒十次
//     - 相机距离 < 10m && 玩家角色
//         中优先级：（200ms 一次） 即一秒五次
//     - 相机距离 10~30m && 玩家角色
//     - 相机距离 < 30m && 怪物 || NPC
//      低优先级（500ms 一次）即一秒俩次
//     - 相机距离 > 30m
    [SerializeField] private float _highPriorityInterval = 0.1f;
    [SerializeField] private float _middlePriorityInterval = 0.2f;
    [SerializeField] private float _lowPriorityInterval = 0.5f;
    
    private float _highTimeCount = 0;
    private float _middleTimeCount = 0;
    private float _lowTimeCount = 0;
    
    private bool _isSyncing;
    
    //是否收到 消息回调
    private bool _isReceiveSyncActorDeltaResponse = true;
    
    private List<Actor> _syncActors = new List<Actor>();
    
    public void Init()
    {
        _isSyncing = false;
    }
    public void StartSync()
    {
        _isSyncing = true;
    }
    
    public void StopSync()
    {
        _isSyncing = false;
    }
    public void DoFixedUpdate()
    {
        if (!_isSyncing)
        {
            return;
        }
        
        float deltaTime = Time.fixedDeltaTime;
        _highTimeCount += deltaTime;
        _middleTimeCount += deltaTime;
        _lowTimeCount += deltaTime;
        
        //如果消息没有回调，不进行同步 锁
        if (!_isReceiveSyncActorDeltaResponse)
        {
            return;
        }
        
        _syncActors.Clear();
        
        
        if (_middleTimeCount >= _middlePriorityInterval)
        {
            SyncMiddlePriority();
            _middleTimeCount = 0;
        }
        
        if (_lowTimeCount >= _lowPriorityInterval)
        {
            SyncLowPriority();
            _lowTimeCount = 0;
        }
        if (_highTimeCount >= _highPriorityInterval)
        {
            SyncHighPriority();
            _highTimeCount = 0;
        }
        if (_syncActors.Count > 0)
        {
            //发送同步信息
            SyncActorToServer();
        }
    }
    
    private void SyncHighPriority()
    {
        //相机距离 < 10m && 玩家角色
        SyncPriorityByCondition((cameraPosition, actor) =>
        {
            return actor.GetActorRoleType() == EActorRoleType.Player && 
                   Vector3.Distance(cameraPosition, actor.transform.position) < 10 ;
        });
    }
    
    private void SyncMiddlePriority()
    {
        //相机距离 10~30m && 玩家角色
        SyncPriorityByCondition((cameraPosition, actor) =>
        {
            return actor.GetActorRoleType() == EActorRoleType.Player && 
                   Vector3.Distance(cameraPosition, actor.transform.position) >= 10 && 
                   Vector3.Distance(cameraPosition, actor.transform.position) < 30;
        });
    }
    
    private void SyncLowPriority()
    {
        //相机距离 > 30m
        SyncPriorityByCondition((cameraPosition, actor) =>
        {
            return Vector3.Distance(cameraPosition, actor.transform.position) >= 30;
        });
    }
    
    private void SyncPriorityByCondition(Func<Vector3,Actor,bool> onCondition)
    {
        //相机距离 < 10m && 玩家角色
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            return;
        }
        Vector3 cameraPosition = mainCamera.transform.position;
        ConcurrentDictionary<int,Actor> actorDic = RoomManager.Instance.GetActorDict();
        foreach (var actor in actorDic)
        {
            if (actor.Value.GetActorState() == EActorState.WaitSync || onCondition.Invoke(cameraPosition,actor.Value))
            {
                SyncActor(actor.Value);
            }
        }
    }
    
    public void SyncActorToServer()
    {
        //发送同步信息
        ConcurrentDictionary<int,Actor> actorDic = RoomManager.Instance.GetOwnerActorDict();
        List<DeltaActorSyncData> deltaActorSyncDatas = new List<DeltaActorSyncData>();
        foreach (var actor in actorDic)
        {
            if (!actor.Value.IsDirty())
            {
                continue;
            }
            //发送同步信息
            DeltaActorSyncData deltaActorSyncData = new DeltaActorSyncData();
            deltaActorSyncData.ActorId = actor.Value.GetActorId();
            Vector3 pos = actor.Value.GetPosition();
            deltaActorSyncData.Pos = ConfigHelper.ConvertUnityVector3ToVector3(pos);
            Vector3 rot = actor.Value.GetRotation();
            deltaActorSyncData.Rot = ConfigHelper.ConvertUnityVector3ToVector3(rot);
            Vector3 speed = actor.Value.GetSpeed();
            deltaActorSyncData.Speed = ConfigHelper.ConvertUnityVector3ToVector3(speed);
            deltaActorSyncData.SyncTime = DateTime.UtcNow.Ticks;//后续都应该用服务器时间
            deltaActorSyncDatas.Add(deltaActorSyncData);
            actor.Value.ClearDirty();
        }
        List<int> inViewActorIds = new List<int>();
        foreach (var actor in _syncActors)
        {
            inViewActorIds.Add(actor.GetActorId());
        }

        _isReceiveSyncActorDeltaResponse = false;
        ClientRequestFunc.SyncActorDeltaRequest(deltaActorSyncDatas,inViewActorIds);
    }
    
    public void ReceiveSyncActorDeltaResponse()
    {
        _isReceiveSyncActorDeltaResponse = true;
    }
    
    public void SyncActor(Actor actor)
    {
        if (!_syncActors.Contains(actor))
        {
            _syncActors.Add(actor);
        }
    }
}