using System;
using System.Collections.Generic;
using ShareProtobuf;
using UnityEngine;
using UnityEngine.UI;

public class RoomViewItem : MonoBehaviour
{
    [SerializeField] RectTransform _root;
    [SerializeField] RoomUserItem _roomUserItemPrefab;
    [SerializeField] Button _selectButton;
    [SerializeField] private GameObject _selectIcon;
    public Action<int> OnSelectRoom { get; set; }
    private SimpleRoomInfo _roomInfo;
    private List<RoomUserItem> _roomUserItems = new List<RoomUserItem>();
    private Queue<RoomUserItem> _roomUserItemsPool = new Queue<RoomUserItem>();

    public void Init()
    {
        _selectIcon.SetActive(false);
        _selectButton.onClick.RemoveAllListeners();
        _selectButton.onClick.AddListener(() =>
        {
            OnSelectRoom?.Invoke(_roomInfo.RoomId);
        });
    }

    public void UpdateRoom(SimpleRoomInfo roomInfo)
    {
        SetSelect(false);
        _roomInfo = roomInfo;
        string playerId = CharacterManager.Instance.PlayerInfo.PlayerId;
        foreach (var roomUserItem in _roomUserItems)
        {
            roomUserItem.gameObject.SetActive(false);
            _roomUserItemsPool.Enqueue(roomUserItem);
        }
        if (roomInfo.Players == null)
        {
            return;
        }
        foreach (var player in roomInfo.Players)
        {
            RoomUserItem item = null;
            if (_roomUserItemsPool.Count > 0)
            {
                item = _roomUserItemsPool.Dequeue();
            }
            else
            {
                item = Instantiate(_roomUserItemPrefab, _root);
            }
            _roomUserItems.Add(item);
            item.gameObject.SetActive(true);
            item.transform.SetAsLastSibling();
            item.UpdateUser(player);
            if (player.UserId == playerId)
            {
                SetSelect(true);
            }
        }
    }
    
    public void SetSelect(bool isSelect)
    {
        _selectIcon.SetActive(isSelect);
    }
    
    public SimpleRoomInfo GetRoomInfo()
    {
        return _roomInfo;
    }
}