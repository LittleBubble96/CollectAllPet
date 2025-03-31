using Cinemachine;
using UnityEngine;

public class RoomSceneController
{
    private ThirdPersonCamera m_FreeLook;
    public void Init()
    {
        //初始化房间场景
        m_FreeLook = GameObject.FindObjectOfType<ThirdPersonCamera>();
    }
    
    public void SetCameraLookAt(Actor target)
    {
        m_FreeLook.target = target.GetLookAtTarget();
    }
}