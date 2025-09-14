using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropController : MonoBehaviour, IPointerClickHandler
{
    public BagSelectManager bagSelector;
    [HideInInspector]public SlotUI pickedSlot = null;
    public Transform player;

    public Vector3IntIntEventSO PlayerDropItemEventSO;
    public VoidEventSO OnPickupEventSO;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (bagSelector.pickedSlot == null || bagSelector.picking != true)
        {
            return;
        }
        PlayerDropItemEventSO.RaiseEvent(player.transform.position,bagSelector.pickedSlot.GetComponent<SlotUI>().item.id,bagSelector.pickedSlot.GetComponent<SlotUI>().num);
        Inventory inventory = new Inventory();
        inventory.itemId = 0;
        inventory.storeNum = 0;
        InventoryLoader.instance.database.inventories[bagSelector.pickedSlot.GetComponent<SlotUI>().index] = inventory;
        OnPickupEventSO.RaiseEvent();
        bagSelector.picking = false;
        bagSelector.pickedSlot = null;
        bagSelector.pickItem.SetActive(false);
    }
}
