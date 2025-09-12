using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public IntAndIntEventSO OnSellEventSO;
    private void OnEnable()
    {
        OnSellEventSO.OnEventRaised += Sell;
    }
    private void OnDisable()
    {
        OnSellEventSO.OnEventRaised -= Sell;
    }
    public void Sell(int itemId,int num)
    {
        Item item = ItemLoader.GetItemById(itemId);
        GeneralDataLoader.instance.database.database.gold += item.price * num;
    }
}
