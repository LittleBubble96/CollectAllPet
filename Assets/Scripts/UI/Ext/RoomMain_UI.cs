using System.Collections.Generic;
using ShareProtobuf;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomMain_UI : UIBase
{
   [SerializeField] private RectTransform roomListRoot;
   [SerializeField] private Button createRoomBtn;
   [SerializeField] private Button joinRoomBtn;
   [SerializeField] private Button refreshRoomListBtn;
   [SerializeField] private RoomViewItem roomViewItemPrefab;
   private List<RoomViewItem> roomViewItems = new List<RoomViewItem>();
   private Queue<RoomViewItem> roomInfoQueue = new Queue<RoomViewItem>();
   
   private object _lock = new object(); 

   private int curSelectRoomId = -1;
   private bool bDirty = false;
   public override void OnInit()
   {
      base.OnInit();
      //初始调用刷新房间列表
      createRoomBtn.onClick.AddListener(() =>
      {
         GameManager.GetUIManager().ShowUI<CreateRoom_PopupUI>();
      });
      joinRoomBtn.onClick.AddListener(() =>
      {
         if (curSelectRoomId != -1)
         {
            GameManager.GetUIManager().ShowLockUI();
            RoomManager.Instance.JoinRoom(curSelectRoomId);
         }
      });
      refreshRoomListBtn.onClick.AddListener(() =>
      {
         GameManager.GetUIManager().ShowLockUI();
         RoomManager.Instance.RefreshRoomList();
      });
   }

   public override void OnShow()
   {
      base.OnShow();
      GameManager.GetAppEventDispatcher().AddEventListener<MultiEvent<List<SimpleRoomInfo>>>(EventName.EVENT_RefreshRoom, OnRefreshRoomList);
      GameManager.GetAppEventDispatcher().AddEventListener<MultiEvent<int, bool>>(EventName.Event_JoinRoomSuccess, OnJoinRoom);
      GameManager.GetUIManager().ShowLockUI();
      RoomManager.Instance.RefreshRoomList();
   }
   

   public override void OnHide()
   {
      base.OnHide();
      GameManager.GetAppEventDispatcher().RemoveEventListener<MultiEvent<List<SimpleRoomInfo>>>(EventName.EVENT_RefreshRoom, OnRefreshRoomList);
      GameManager.GetAppEventDispatcher().RemoveEventListener<MultiEvent<int, bool>>(EventName.Event_JoinRoomSuccess, OnJoinRoom);
   }

   public override void DoUpdate(float dt)
   {
      base.DoUpdate(dt);
      if (bDirty)
      {
         bDirty = false;
         for (int i = roomViewItems.Count - 1; i >= 0; i--)
         {
            roomViewItems[i].SetSelect(roomViewItems[i].GetRoomInfo().RoomId == curSelectRoomId);
         }
      }
   }
   
   private void OnJoinRoom(MultiEvent<int, bool> obj)
   {
      if (obj.Value1)
      {
         RoomManager.Instance.RefreshRoomList();
      }
      GameManager.GetUIManager().HideLockUI();
   }

   private void OnRefreshRoomList(MultiEvent<List<SimpleRoomInfo>> obj)
   {
      //清空房间列表
      foreach (var roomViewItem in roomViewItems)
      {
         roomViewItem.gameObject.SetActive(false);
         roomInfoQueue.Enqueue(roomViewItem);
      }
      roomViewItems.Clear();
      //刷新房间列表
      for (int i = 0; i < obj.Value.Count; i++)
      {
         RoomViewItem item = null;
         if (roomInfoQueue.Count > 0)
         {
            item = roomInfoQueue.Dequeue();
         }
         else
         {
            item = Instantiate(roomViewItemPrefab, roomListRoot);
            item.Init();
            item.OnSelectRoom = (roomId) =>
            {
               this.curSelectRoomId = roomId;
               bDirty = true;
            };
         }
         roomViewItems.Add(item);
         item.gameObject.SetActive(true);
         item.transform.SetSiblingIndex(i);
         item.UpdateRoom(obj.Value[i]);
      }
      GameManager.GetUIManager().HideLockUI();
   }
   
}
