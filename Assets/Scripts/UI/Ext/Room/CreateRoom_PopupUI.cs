using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom_PopupUI : UIBase
{
    [SerializeField] private TMP_InputField _roomNameInput;
    [SerializeField] private TMP_Dropdown _maxPlayerCountDropdown;
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private Button _closeButton;
    
    public override void OnInit()
    {
        base.OnInit();
        _createRoomButton.onClick.AddListener(() =>
        {
            string roomName = _roomNameInput.text;
            int maxPlayerCount = _maxPlayerCountDropdown.value;
            GameManager.GetUIManager().ShowLockUI();
            RoomManager.Instance.CreateRoom(roomName, maxPlayerCount);
        });
        _closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }
    
    public override void OnShow()
    {
        base.OnShow();
        GameManager.GetAppEventDispatcher().AddEventListener<MultiEvent<bool,string>>(EventName.Event_CreateRoomSuccess, OnCreateRoomSuccess);
    }
    
    public override void OnHide()
    {
        base.OnHide();
        GameManager.GetAppEventDispatcher().RemoveEventListener<MultiEvent<bool,string>>(EventName.Event_CreateRoomSuccess, OnCreateRoomSuccess);
    }

    private void OnCreateRoomSuccess(MultiEvent<bool,string> obj)
    {
        if (obj.Value)
        {
            Hide();
            RoomManager.Instance.RefreshRoomList();
        }
        else
        {
            Debug.Log(obj.Value1);
            GameManager.GetUIManager().HideLockUI();
        }
        
    }

    
}