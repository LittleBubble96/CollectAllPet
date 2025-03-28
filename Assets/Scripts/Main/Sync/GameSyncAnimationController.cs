using System.Collections.Generic;
using ShareProtobuf;

public class GameSyncAnimationController
{
    private float _syncAnimationTime;
    
    private float _syncAnimationTimer;
    
    private List<DeltaActorAnimationSyncData> _deltaActorAnimationSyncDatas = new List<DeltaActorAnimationSyncData>();
    
    public void Init(float syncAnimationTime)
    {
        _syncAnimationTime = syncAnimationTime;
    }
    
    public void DoFixedUpdate()
    {
        _syncAnimationTimer += UnityEngine.Time.fixedDeltaTime;
        if (_syncAnimationTimer >= _syncAnimationTime)
        {
            _syncAnimationTimer = 0;
            if (_deltaActorAnimationSyncDatas.Count > 0)
            {
                SyncAnimation();
            }
        }
    }
    
    public void SyncAnimation()
    {
        if (_deltaActorAnimationSyncDatas.Count > 0)
        {
            ClientRequestFunc.SyncActorAnimationRequest(_deltaActorAnimationSyncDatas);
            _deltaActorAnimationSyncDatas.Clear();
        }
    }
    
    public void SetAnimationFloatParams(int actorId, string paramName, float value)
    {
        _deltaActorAnimationSyncDatas.Add(new DeltaActorAnimationSyncData()
        {
            ActorId = actorId,
            AnimationParamName = paramName,
            // float: 0f
            AnimationParamValue = GameConst.AnimationPreNameFloat + GameConst.ColonStr + value
        });
    }
    
    public void SetAnimationIntParams(int actorId, string paramName, int value)
    {
        _deltaActorAnimationSyncDatas.Add(new DeltaActorAnimationSyncData()
        {
            ActorId = actorId,
            AnimationParamName = paramName,
            // int: 1
            AnimationParamValue = GameConst.AnimationPreNameInt + GameConst.ColonStr + value
        });
    }
    
    public void SetAnimationBoolParams(int actorId, string paramName, bool value)
    {
        _deltaActorAnimationSyncDatas.Add(new DeltaActorAnimationSyncData()
        {
            ActorId = actorId,
            AnimationParamName = paramName,
            // bool: 1
            AnimationParamValue = GameConst.AnimationPreNameBool + GameConst.ColonStr + (value ? 1 : 0)
        });
    }
}