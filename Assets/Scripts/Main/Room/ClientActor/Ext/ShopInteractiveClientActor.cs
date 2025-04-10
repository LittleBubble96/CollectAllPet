using System;

public class ShopInteractiveClientActor : ClientActor
{
    private bool isOpen = false;
    private void OnEnable()
    {
        if (GameManager.Instance == null)
        {
            return;
        }
        GameManager.GetAppEventDispatcher().AddEventListener<EventType>(EventName.Event_ShopClose,OnCloseShop);
    }

   

    private void OnDisable()
    {
        GameManager.GetAppEventDispatcher().RemoveEventListener<EventType>(EventName.Event_ShopClose,OnCloseShop);
    }
    
    private void OnCloseShop<TEvent>(TEvent obj) where TEvent : EventType
    {
        isOpen = false;
    }

    public override void HandleInteractive()
    {
        if (isOpen)
        {
            return;
        }
        base.HandleInteractive();
        //打开商店界面
        GameManager.GetUIManager().ShowUI<GameShop_PopupUI>();
        isOpen = true;
    }
}