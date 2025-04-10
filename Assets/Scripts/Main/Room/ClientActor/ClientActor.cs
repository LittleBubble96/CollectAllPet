using System;
using UnityEngine;

public class ClientActor : MonoBehaviour , IInteractiveHandle
{
    [SerializeField] private float m_fVisibleDistance = 10f;
    [SerializeField] private float m_fVisibleAngle = 60f;
    
    private bool bPlayerEnter = false;
    
    private bool bEntered = false;

    private void Awake()
    {
        //添加一个SphereCollider 
        SphereCollider sphereCollider = gameObject.GetComponent<SphereCollider>();
        if (sphereCollider == null)
        {
            sphereCollider = gameObject.AddComponent<SphereCollider>();
        }
        sphereCollider.isTrigger = true;
        sphereCollider.radius = m_fVisibleDistance;
    }



    private void Update()
    {
        if (!bPlayerEnter)
        {
            return;
        }
        Actor playerActor = RoomManager.Instance.GetRefActor();
        if (playerActor == null)
        {
            return;
        }
        //获取玩家位置 和 交互物体位置 看朝向夹角是否在小于 m_fVisibleAngle
        Vector3 playerPos = playerActor.transform.position;
        Vector3 actorPos = transform.position;
        Vector3 dir = (actorPos - playerPos).normalized;
        Vector3 forward = playerActor.transform.forward;
        float angle = Vector3.Angle(dir, forward);
        //Debug.Log($"angle: {angle} m_fVisibleAngle: {m_fVisibleAngle}");
        //如果在可见范围内
        if (angle < m_fVisibleAngle)
        {
            if (!bEntered)
            {
                GameManager.GetAppEventDispatcher().BroadcastListener(EventName.Event_EnterInteractableRange,this as IInteractiveHandle);
                bEntered = true;
            }
        }
        else
        {
            if (bEntered)
            {
                GameManager.GetAppEventDispatcher().BroadcastListener(EventName.Event_LeaveInteractableRange, this as IInteractiveHandle);
                bEntered = false;
            }
        }
    }
    
    //触发器检查 进入得Actor
    private void OnTriggerEnter(Collider other)
    {
        //获取Actor组件
        Actor actor = other.GetComponent<Actor>();
        if (actor != null && RoomManager.Instance.GetRefActorId() == actor.GetActorId())
        {
            //设置可见
            bPlayerEnter = true;
        }
    }
    //触发器检查 离开得Actor
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //获取Actor组件
            Actor actor = other.GetComponent<Actor>();
            if (actor != null && RoomManager.Instance.GetRefActorId() == actor.GetActorId())
            {
                //设置不可见
                bPlayerEnter = false;
            }
        }
    }

    //触发器检查 进入得Actor
    public virtual void HandleInteractive()
    {
        
    }

    public Vector3 GetInteractivePosition()
    {
        return transform.position;
    }
    
    public string GetInteractiveText()
    {
        return "Press F";
    }
}