using ShareProtobuf;
using UnityEngine;
public enum EActorState
{
    None,
    WaitSync,
    Syncing,
    Ready,
}

public enum EActorRoleType
{
    None,
    Player,
    NPC,
    Monster,
    Interactive,
}
public class Actor : MonoBehaviour
{
    private GameActorInfo actorInfo;
    private EActorState actorState = EActorState.None;
    
    private Vector3 clientPosition;
    
    private Vector3 clientRotation;
    
    private Vector3 clientSpeed;
    
    private bool bDirty = false;
    public void InitActor(GameActorInfo inActorInfo)
    {
        // Init Actor
        actorInfo = inActorInfo;
        actorState = EActorState.WaitSync;
        gameObject.name = actorInfo.ActorName;
    }
    
    public int GetActorId()
    {
        return actorInfo.RefActorId;
    }

    public EActorState GetActorState()
    {
        return actorState;
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