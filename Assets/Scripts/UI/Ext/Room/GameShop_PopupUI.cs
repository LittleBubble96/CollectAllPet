using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameShop_PopupUI : UIBase
{
    [SerializeField] private TextMeshProUGUI m_totalMoneyText;
    [SerializeField] private TextMeshProUGUI m_totalRemainText;
    [SerializeField] private Button m_closeButton;
    [SerializeField] private Button m_buyButton;
    
    private GameShopThingItem[] m_gameShopThingItems;
    
    private int m_totalMoney;

    public override void OnInit()
    {
        base.OnInit();
        m_gameShopThingItems = GetComponentsInChildren<GameShopThingItem>(true);
        foreach (var thing in m_gameShopThingItems)
        {
            thing.OnCountChange += OnCountChange;
        }
        m_closeButton.onClick.AddListener(Hide);
    }

    private void OnCountChange(int count)
    {
        m_totalMoney = 0;
        foreach (var thing in m_gameShopThingItems)
        {
            m_totalMoney += thing.Count * thing.PayNum;
        }
        m_totalMoneyText.text = m_totalMoney.ToString();
        UpdateRemainCount();
    }


    private void UpdateRemainCount()
    {
        int roleGold = RoomManager.Instance.GetGold();
        m_totalRemainText.text = (roleGold - m_totalMoney).ToString();
    }

    public override void OnShow()
    {
        base.OnShow();
        GameManager.GetAppEventDispatcher().AddEventListener<MultiEvent<int>>(EventName.Event_UpdateGold, GoldUpdate);
    }
    
    public override void OnHide()
    {
        base.OnHide();
        GameManager.GetAppEventDispatcher().RemoveEventListener<MultiEvent<int>>(EventName.Event_UpdateGold, GoldUpdate);
        //广播商店关闭
        GameManager.GetAppEventDispatcher().BroadcastListener(EventName.Event_ShopClose);
    }

    private void GoldUpdate(MultiEvent<int> obj)
    {
        UpdateRemainCount();
    }
}
