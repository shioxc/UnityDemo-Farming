using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BagSlotSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    private GameObject bag;
    public bool isShopping = false;
    private ShopManager shopManager;
    public IntAndIntEventSO OnSellEventSO;
    private void Awake()
    {
        bag = transform.parent.parent.gameObject;
    }
    private void SetShopManager(GameObject obj)
    {
        shopManager = obj.GetComponent<ShopManager>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isShopping)
            bag.GetComponent<BagSelectManager>()?.ClickItem(gameObject);
        else
        {
            SlotUI slotUI = GetComponent<SlotUI>();
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                OnSellEventSO.RaiseEvent(slotUI.item.id,slotUI.num);
                InventoryLoader.instance.ReduceItem(slotUI.index, slotUI.num);
                
            }
            else if(eventData.button== PointerEventData.InputButton.Right)
            {
                OnSellEventSO.RaiseEvent(slotUI.item.id, 1);
                InventoryLoader.instance.ReduceItem(slotUI.index, 1);
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        bag.GetComponent<BagSelectManager>()?.SelectSlot(gameObject);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        bag.GetComponent<BagSelectManager>()?.UnSelectSlot();
    }
}
