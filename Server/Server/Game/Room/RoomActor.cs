

using System.Collections.Concurrent;
using Newtonsoft.Json;
using ShareProtobuf;

public class RoomActor
{ 
    public int ActorId { get; private set; }

    public RoomWorld Room { get; private set; }

    public EActorRoleType Role { get; private set; }
    public Vector3 Pos { get; private set; }
    public Vector3 Rot { get; private set; }
    
    public Vector3 Speed { get; private set; }
    
    public long SyncTime { get; set; }

    
    public string OwnerPlayerId { get; private set; }
    
    public int ActorCfgId { get; private set; }
    
    public string ActorName { get; private set; }
    
    protected ConcurrentDictionary<int,object> ActorAttributes = new ConcurrentDictionary<int, object>();
    //脏属性类型
    protected int dirtyAttributeType = -1;
    
    private bool isDestroy = false;
    public Action<RoomActor> OnDestroyAction { get; set; }
    public bool IsDestroy
    {
        get { return isDestroy; }
        set { isDestroy = value; }
    }


    public void Init(string playerId ,RoomWorld room , EActorRoleType roleType, int actorCfgId ,int actorId, Vector3 pos, Vector3 rot)
    {
        ActorId = actorId;
        Room = room;
        Role = roleType;
        Pos = pos;
        Rot = rot;
        OwnerPlayerId = playerId;
        ActorCfgId = actorCfgId;
        ActorName = GetActorName();
        OnInit();
    }


    private string GetActorName()
    {
        string preName = "";
        switch (Role)
        {
            case EActorRoleType.Player:
                preName = "Player";
                break;
            case EActorRoleType.Monster:
                preName = "Monster";
                break;
            case EActorRoleType.Interactive:
                preName = "Interactive";
                break;
            case EActorRoleType.BreakInteractive:
                preName = "BreakInteractive";
                break;
        }
        return preName + "_" + ActorCfgId + "_" + ActorId + "_" + OwnerPlayerId;
    }
    
    
    public void SyncPos(Vector3 pos)
    {
        Pos = pos;
    }
    
    public void SyncRot(Vector3 rot)
    {
        Rot = rot;
    }
    
    public void SyncSpeed(Vector3 speed)
    {
        Speed = speed;
    }
    
    public void SyncServeTime()
    {
        SyncTime = DateTime.UtcNow.Ticks;
    }
    
    public float GetDistance(RoomActor actor)
    {
        return Vector3.Distance(Pos, actor.Pos);
    }

    public string GetDirtyAttributeJson()
    {
        Dictionary<int,object> d = new Dictionary<int,object>();
        foreach (var attribute in ActorAttributes)
        {
            if ((attribute.Key & dirtyAttributeType)  > 0)
            {
                //TODO 这里需要序列化成json
                d [attribute.Key] = attribute.Value;
            }
        }
        return JsonConvert.SerializeObject(d);
    }
    
    public string GetAllDirtyAttributeJson()
    {
        return JsonConvert.SerializeObject(ActorAttributes);
    }

    public virtual void OnInit()
    {
        
    }

    public void Destroy()
    {
        IsDestroy = true;
        OnDestroyAction?.Invoke(this);
        Room.DestroyActor(this);
        OnDestroy();
    }

    protected virtual void OnDestroy()
    {
    
    }
    
    
    #region 属性
    public void AddAttribute(int attributeId, object value)
    {
        //添加属性
        ActorAttributes.TryAdd(attributeId, value);
    }
    
    public int GetIntAttribute(int attributeId)
    {
        if (ActorAttributes.TryGetValue(attributeId, out object value))
        {
            return Convert.ToInt32(value);
        }
        return 0;
    }
    
    public float GetFloatAttribute(int attributeId)
    {
        if (ActorAttributes.TryGetValue(attributeId, out object value))
        {
            return Convert.ToSingle(value);
        }
        return 0;
    }
    
    //更新属性 并标记为脏
    public void UpdateAttribute(int attributeId, object value)
    {
        //更新属性
        ActorAttributes[attributeId] = value;
        dirtyAttributeType = dirtyAttributeType | attributeId;
    }
    
    public bool RoomWorldAttIsDirty()
    {
        return dirtyAttributeType > 0;
    }
    

    #endregion

}