using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagUI : MonoBehaviour
{
    public GameObject bagUI;

    public GameobjectEventSO GameobjectEventSO;
    public PlayerController player;
    public GoodsListEventSO OnOpenShopEventSO;
    public bool isShopping =false;
    private void OnEnable()
    {
        OnOpenShopEventSO.OnEventRaised += OpenShop;
    }
    private void Onenable()
    {
        OnOpenShopEventSO.OnEventRaised -= OpenShop;
    }
    public void Open()
    {
        GameobjectEventSO.RaiseEvent(bagUI);
        Time.timeScale = 0f;
        gameObject.GetComponent<InventoryUI>()?.RefreshBagUI();
    }
    public void OpenShop(List<Goods> list)
    {
        bagUI.transform.Find("Extra").GetComponent<ShopUI>().OpenShop(list);
        isShopping = true;
        OpenExtra();
    }
    public void OpenExtra()//打开商店或箱子
    {
        if (bagUI.transform.Find("Extra").gameObject.activeSelf) return;
        player.inputControl.Gameplay.OpenBag.Disable();
        player.curUI = bagUI;
        Time.timeScale = 0f;
        bagUI.SetActive(true);
        foreach(Transform bagList in bagUI.transform)
        {
            if (!bagList.name.Contains("List"))
            {
                continue;
            }
            foreach(Transform slot in bagList)
            {
                slot.GetComponent<BagSlotSelect>().isShopping = true;
            }
            Vector3 pos = bagList.position;
            pos.y -= 478;
            bagList.position = pos;
        }
        GetComponent<InventoryUI>().RefreshBagUI();
        bagUI.transform.Find("Extra").gameObject.SetActive(true);
        
    }
    public void CloseExtra()
    {
        if (!bagUI.transform.Find("Extra").gameObject.activeSelf) return;
        player.inputControl.Gameplay.OpenBag.Enable();
        bagUI.transform.Find("Extra").gameObject.SetActive(false);
        foreach (Transform bagList in bagUI.transform)
        {
            if (!bagList.name.Contains("List"))
            {
                continue;
            }
            foreach (Transform slot in bagList)
            {
                slot.GetComponent<BagSlotSelect>().isShopping = false;
            }
            Vector3 pos = bagList.position;
            pos.y += 478;
            bagList.position = pos;
        }
    }
}
