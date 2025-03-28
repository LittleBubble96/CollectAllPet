using Cinemachine;
using UnityEngine;

public class RoomSceneController
{
    private CinemachineFreeLook m_FreeLook;
    public void Init()
    {
        //初始化房间场景
        m_FreeLook = GameObject.FindObjectOfType<CinemachineFreeLook>();
    }
    
    public void SetCameraLookAt(Actor target)
    {
        m_FreeLook.LookAt = target.GetLookAtTarget();
        m_FreeLook.Follow = target.GetLookAtTarget();
    }
}