using System;
using UnityEngine;

public class ClientActor : MonoBehaviour
{
    [SerializeField] private float m_fVisibleDistance = 10f;
    [SerializeField] private float m_fVisibleAngle = 60f;
    
    private bool bBecameVisible = false;

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

    //相机是否可见
    public bool IsVisible => bBecameVisible;
    
    
    private void OnBecameInvisible()
    {
        bBecameVisible = false;
    }
    
    private void OnBecameVisible()
    {
        bBecameVisible = true;
    }
    
    //触发器检查 进入得Actor
    private void OnTriggerEnter(Collider other)
    {
        //获取Actor组件
        Actor actor = other.GetComponent<Actor>();
        if (actor != null)
        {
            //设置可见
        }
    }
    //触发器检查 离开得Actor
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //获取Actor组件
            Actor actor = other.GetComponent<Actor>();
            if (actor != null)
            {
                //设置不可见
            }
        }
    }
}