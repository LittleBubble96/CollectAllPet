using ShareProtobuf;
using UnityEngine;
public enum EActorState
{
    None,
    WaitSync, // 创建好时  等待更新
    Syncing,  // 同步位置到服务器
    Ready,    // 同步完成  可以操作
}

public enum EActorRoleType
{
    None,
    Player,
    NPC,
    Monster,
    Interactive,
}

public enum CAP_ControlMode
{
    /// <summary>
    /// Up moves the character forward, left and right turn the character gradually and down moves the character backwards
    /// </summary>
    Player,
    /// <summary>
    /// Character freely moves in the chosen direction from the perspective of the camera
    /// </summary>
    Server
}

public class Actor : MonoBehaviour
{
    private GameActorInfo actorInfo;
    private EActorState actorState = EActorState.None;
    
    private Vector3 clientPosition;
    
    private Vector3 clientRotation;
    
    private Vector3 clientSpeed;
    
    private Vector3 serverPosition;
    private Vector3 serverRotation;
    private Vector3 serverSpeed;
    
    private bool bDirty = false;
    
    private CAP_ControlMode m_controlMode = CAP_ControlMode.Player;

    public void InitActor(GameActorInfo inActorInfo)
    {
        // Init Actor
        actorInfo = inActorInfo;
        actorState = EActorState.WaitSync;
        gameObject.name = actorInfo.ActorName;
        clientPosition = transform.position;
        clientRotation = transform.eulerAngles;
        clientSpeed = Vector3.zero;
    }

    public CAP_ControlMode GetControlMode()
    {
        return m_controlMode;
    }
    
    public void SetControlMode(CAP_ControlMode controlMode)
    {
        // m_controlMode = controlMode;
        // if (m_controlMode == CAP_ControlMode.Player)
        // {
        //     SetActorState(EActorState.Ready);
        // }
    }

    public int GetActorId()
    {
        return actorInfo.RefActorId;
    }

    public EActorState GetActorState()
    {
        return actorState;
    }
    
    public void SetActorState(EActorState state)
    {
        actorState = state;
    }

    public EActorRoleType GetActorRoleType()
    {
        return (EActorRoleType)actorInfo.ActorRoleType;
    }
    
    public bool IsOwnerPlayer()
    {
        return actorInfo.OwnerPlayerId == CharacterManager.Instance.PlayerInfo.PlayerId;
    }
    
    public void SetPosition(Vector3 position)
    {
        clientPosition = position;
        MakeDirty();
    }
    
    public Vector3 GetPosition()
    {
        return clientPosition;
    }
    
    public void SetRotation(Vector3 rotation)
    {
        clientRotation = rotation;
        MakeDirty();
    }
    
    public Vector3 GetRotation()
    {
        return clientRotation;
    }
    
    public void SetSpeed(Vector3 speed)
    {
        clientSpeed = speed;
        MakeDirty();
    }
    
    public Vector3 GetSpeed()
    {
        return clientSpeed;
    }
    
    //Server
    public void SetServerPosition(Vector3 position)
    {
        serverPosition = position;
        transform.position = serverPosition;
    }
    
    public Vector3 GetServerPosition()
    {
        return serverPosition;
    }
    
    public void SetServerRotation(Vector3 rotation)
    {
        serverRotation = rotation;
        transform.eulerAngles = serverRotation;
    }
    
    public Vector3 GetServerRotation()
    {
        return serverRotation;
    }
    
    public void SetServerSpeed(Vector3 speed)
    {
        serverSpeed = speed;
    }
    
    
    public void MakeDirty()
    {
        bDirty = true;
    }
    
    public bool IsDirty()
    {
        return bDirty;
    }
    
    public void ClearDirty()
    {
        bDirty = false;
    }
    
}