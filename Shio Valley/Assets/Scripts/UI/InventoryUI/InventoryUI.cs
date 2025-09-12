using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;

public class InventoryUI : MonoBehaviour
{

    public GameobjectEventSO onBagSlotChangedEventSO;
    public VoidEventSO OnPickupEventSO;
    private void OnEnable()
    {
        onBagSlotChangedEventSO.OnEventRaised += RefreshSlot;
        OnPickupEventSO.onEventRaised += RefreshCurList;
        OnPickupEventSO.onEventRaised += RefreshBagUI;
        RefreshCurList();
    }
    private void OnDisable()
    {
        OnPickupEventSO.onEventRaised -= RefreshCurList;
        OnPickupEventSO.onEventRaised -= RefreshBagUI;
        onBagSlotChangedEventSO.OnEventRaised -= RefreshSlot;
    }

    public void RefreshCurList()
    {
        ItemDatabase itemDatabase = ItemLoader.GetItemDatabase();
        int index = 0;
        foreach (Transform childTransform in transform.Find("CurList").transform)
        {
            if (childTransform.name == "ProfileTip") break;
            int curIndex = index;
            TMP_Text num = childTransform.Find("Num").GetComponent<TMP_Text>();
            Image itemIcon = childTransform.Find("ItemIcon").GetComponent<Image>();
            Item itemInfo = itemDatabase.items.Find(i => i.id == InventoryLoader.instance.database.inventories[index].itemId);
            SlotUI obj = childTransform.GetComponent<SlotUI>();
            obj.index = curIndex;
            if (itemInfo == null)
            {
                obj.item = null;
                obj.num = 0;
                itemIcon.gameObject.SetActive(false);
                num.gameObject.SetActive(false);
            }
            else
            {
                if(obj != null)
                {

                    obj.item = itemInfo;
                    obj.num = InventoryLoader.instance.database.inventories[index].storeNum;
                }
                num.text = $"{InventoryLoader.instance.database.inventories[index].storeNum}";
                SpriteCache.GetSprite(itemInfo.iconKey, sprite =>
                {
                    if (itemIcon != null)
                    {
                        itemIcon.sprite = sprite;
                    }
                    itemIcon.gameObject.SetActive(true);
                });
                num.gameObject.SetActive(true);
            }
            index++;
        }
    }
    public void RefreshBagUI()
    {
        if (!transform.Find("Bag").gameObject.activeSelf) return;
        ItemDatabase itemDatabase = ItemLoader.GetItemDatabase();
        int index = 0;
        foreach (Transform childTransform in transform.Find("Bag").transform)
        {
            if (!childTransform.name.Contains("List"))
            {
                continue;
            }
            foreach (Transform childchildTransform in childTransform)
            {
                int curIndex = index;
                TMP_Text num = childchildTransform.Find("Num").GetComponent<TMP_Text>();
                Image itemIcon = childchildTransform.Find("ItemIcon").GetComponent<Image>();
                Item itemInfo = itemDatabase.items.Find(i => i.id == InventoryLoader.instance.database.inventories[index].itemId);
                SlotUI obj = childchildTransform.GetComponent<SlotUI>();
                obj.index = curIndex;
                if (itemInfo == null)
                {
                    obj.item = null;
                    obj.num = 0;
                    itemIcon.gameObject.SetActive(false);
                    num.gameObject.SetActive(false);
                }
                else
                {
                    if (obj != null)
                    {
                        obj.item = itemInfo;
                        obj.num = InventoryLoader.instance.database.inventories[index].storeNum;
                    }
                    num.text = $"{InventoryLoader.instance.database.inventories[index].storeNum}";
                    SpriteCache.GetSprite(itemInfo.iconKey, sprite =>
                    {
                        if (itemIcon != null)
                        {
                            itemIcon.sprite = sprite;
                        }
                    });
                    if(transform.Find("Bag").GetComponent<BagSelectManager>().pickedSlot != childchildTransform.gameObject)
                    {
                        itemIcon.gameObject.SetActive(true);
                        num.gameObject.SetActive(true);
                    }
                }
                index++;
            }
        }
    }

    private void RefreshSlot(GameObject obj)
    {
        ItemDatabase itemDatabase = ItemLoader.GetItemDatabase();
        TMP_Text num = obj.transform.Find("Num").GetComponent<TMP_Text>();
        Image itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
        SlotUI slot = obj.GetComponent<SlotUI>();
        Item itemInfo = itemDatabase.items.Find(i => i.id == InventoryLoader.instance.database.inventories[slot.index].itemId);
        if (itemInfo == null)
        {
            slot.item = null;
            slot.num = 0;
            itemIcon.gameObject.SetActive(false);
            num.gameObject.SetActive(false);
        }
        else
        {
            if (obj != null)
            {
                slot.item = itemInfo;
                slot.num = InventoryLoader.instance.database.inventories[slot.index].storeNum;
            }
            num.text = $"{InventoryLoader.instance.database.inventories[slot.index].storeNum}";
            SpriteCache.GetSprite(itemInfo.iconKey, sprite =>
            {
                if (itemIcon != null)
                {
                    itemIcon.sprite = sprite;
                }
                itemIcon.gameObject.SetActive(true);
            });
            num.gameObject.SetActive(true);
        }
        if(transform.Find("Bag").gameObject.activeSelf)
        {
            RefreshBagUI();
        }
        RefreshCurList();
    }
}
