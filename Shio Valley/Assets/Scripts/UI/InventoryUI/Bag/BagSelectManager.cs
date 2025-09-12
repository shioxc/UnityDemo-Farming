using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BagSelectManager : MonoBehaviour
{
    public GameObject selectedSlot;

    [SerializeField]private GameObject profile;
    public bool picking;
    public GameObject pickedSlot;
    private int index;
    public GameObject pickItem;

    public GameobjectEventSO onBagSlotChangedEventSO;
    public DropController dropController;

    private void OnEnable()
    {
        picking = false;
    }

    private void OnDisable()
    {
        UnSelectSlot();
        if(pickedSlot != null)
        {
            pickedSlot.transform.Find("ItemIcon")?.gameObject.SetActive(true);
            pickedSlot.transform.Find("Num")?.gameObject.SetActive(true);
        }
        pickedSlot = null;
        picking = false;
        pickItem.SetActive(false);
    }
    public void SelectSlot(GameObject slot)
    {
        UnSelectSlot();
        selectedSlot = slot;
        if(!picking)
        {
            Item item = selectedSlot.GetComponent<SlotUI>()?.item;
            if(item != null)
            {
                profile.GetComponent<ProfileTip>()?.SetProfile(item);
                profile.SetActive(true);
                profile.GetComponent<ProfileTip>()?.RefreshUI();
            }
        }
        slot.transform.Find("SelectedTip")?.gameObject.SetActive(true);
    }

    public void UnSelectSlot() 
    {
        if (selectedSlot == null) return;
        profile.SetActive(false);
        selectedSlot.transform.Find("SelectedTip")?.gameObject.SetActive(false);
        selectedSlot = null;
    }

    public void ClickItem(GameObject slot)
    {
        if (picking)
        {
            pickItem.SetActive(false);
            picking = false;
            int index = pickedSlot.GetComponent<SlotUI>().index;
            int index1 = selectedSlot.GetComponent<SlotUI>().index;
            (InventoryLoader.instance.database.inventories[index], InventoryLoader.instance.database.inventories[index1]) = (InventoryLoader.instance.database.inventories[index1], InventoryLoader.instance.database.inventories[index]);
            onBagSlotChangedEventSO.RaiseEvent(pickedSlot);
            onBagSlotChangedEventSO.RaiseEvent(selectedSlot);
            pickedSlot =null;
            dropController.pickedSlot = null;
        }
        else
        {
            if (slot.GetComponent<SlotUI>()?.item == null) return;
            profile.SetActive(false);
            picking = true;
            pickedSlot = slot;
            pickItem.GetComponent<Image>().sprite = selectedSlot.transform.Find("ItemIcon")?.GetComponent<Image>()?.sprite;
            pickItem.SetActive(true);
            slot.transform.Find("ItemIcon")?.gameObject.SetActive(false);
            slot.transform.Find("Num")?.gameObject.SetActive(false);
            dropController.pickedSlot = slot.GetComponent<SlotUI>();
        }
    }
}
