using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomGame_UI :UIBase 
{
    //目前这个交互物体不会消失
    private List<IInteractiveHandle> _interactiveHandles = new List<IInteractiveHandle>();
    
    private IInteractiveHandle _currentInteractiveHandle;
    
    [SerializeField] private Button _interactButton;
    [SerializeField] private TextMeshProUGUI _interactText;
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private TextMeshProUGUI _diamondText;
    [SerializeField] private TextMeshProUGUI _deltaGoldText;

    public override void OnInit()
    {
        base.OnInit();
        _interactButton.gameObject.SetActive(false);
        _interactButton.onClick.AddListener(OnInteractButtonClick);
    }

    public override void OnShow()
    {
        base.OnShow();
        //注册进入或者离开交互物体方法
        GameManager.GetAppEventDispatcher().AddEventListener<MultiEvent<IInteractiveHandle>>(EventName.Event_EnterInteractableRange, EnterInteractableRange);
        //注册进入或者离开交互物体方法
        GameManager.GetAppEventDispatcher().AddEventListener<MultiEvent<IInteractiveHandle>>(EventName.Event_LeaveInteractableRange, LeaveInteractableRange);
        //金币更新
        GameManager.GetAppEventDispatcher().AddEventListener<MultiEvent<int>>(EventName.Event_UpdateGold, GoldUpdate);
        //delta金币更新
        GameManager.GetAppEventDispatcher().AddEventListener<MultiEvent<int>>(EventName.Event_UpdateGoldDelta, DeltaGoldUpdate);
        //钻石更新
        GameManager.GetAppEventDispatcher().AddEventListener<MultiEvent<int>>(EventName.Event_UpdateDiamond, DiamondUpdate);
    }


    public override void OnHide()
    {
        base.OnHide();
        //注销进入或者离开交互物体方法
        GameManager.GetAppEventDispatcher().RemoveEventListener<MultiEvent<IInteractiveHandle>>(EventName.Event_EnterInteractableRange, EnterInteractableRange);
        //注销进入或者离开交互物体方法  
        GameManager.GetAppEventDispatcher().RemoveEventListener<MultiEvent<IInteractiveHandle>>(EventName.Event_LeaveInteractableRange, LeaveInteractableRange);
        //金币更新
        GameManager.GetAppEventDispatcher().RemoveEventListener<MultiEvent<int>>(EventName.Event_UpdateGold, GoldUpdate);
        //delta金币更新
        GameManager.GetAppEventDispatcher().RemoveEventListener<MultiEvent<int>>(EventName.Event_UpdateGoldDelta, DeltaGoldUpdate);
        //钻石更新
        GameManager.GetAppEventDispatcher().RemoveEventListener<MultiEvent<int>>(EventName.Event_UpdateDiamond, DiamondUpdate);
    }

    public override void DoUpdate(float dt)
    {
        base.DoUpdate(dt);
        if (_currentInteractiveHandle != null)
        {
            Vector3 worldPos = _currentInteractiveHandle.GetInteractivePosition();
            Vector3 screenPos = UIHelper.WorldToUIPos(worldPos);
            _interactButton.transform.position = screenPos;
            if (Input.GetKeyDown(KeyCode.F))
            {
                _currentInteractiveHandle.HandleInteractive();
            }
        }
        
    }
    
    private void GoldUpdate(MultiEvent<int> obj)
    {
        _goldText.text = obj.Value.ToString();
    }
    
    private void DiamondUpdate(MultiEvent<int> obj)
    {
        _diamondText.text = obj.Value.ToString();
    }

    private void DeltaGoldUpdate(MultiEvent<int> obj)
    {
        _deltaGoldText.text = obj.Value.ToString();
    }

    private void EnterInteractableRange(MultiEvent<IInteractiveHandle> obj)
    {
        _interactiveHandles.Add(obj.Value);
        ActiveInteractiveHandle();
    }
    
    private void LeaveInteractableRange(MultiEvent<IInteractiveHandle> obj) 
    {
        _interactiveHandles.Remove(obj.Value);
        ActiveInteractiveHandle();

    }

    private void ActiveInteractiveHandle()
    {
        _currentInteractiveHandle = null;
        if (_interactiveHandles.Count != 0)
        {
            _currentInteractiveHandle = _interactiveHandles.Last();
        }

        //交互按钮显示
        _interactButton.gameObject.SetActive(_currentInteractiveHandle != null);
        _interactText.text = _currentInteractiveHandle != null ? _currentInteractiveHandle.GetInteractiveText() : string.Empty;
    }
    
    private void OnInteractButtonClick()
    {
        if (_currentInteractiveHandle != null)
        {
            _currentInteractiveHandle.HandleInteractive();
        }
    }
}