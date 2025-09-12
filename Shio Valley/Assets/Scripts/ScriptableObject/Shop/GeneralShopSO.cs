using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Goods
{
    public int itemId;
    public int num;
    public int price;
}
public abstract class GeneralShopSO : ScriptableObject
{
    public List<Goods> goodslist;

    public virtual List<Goods> Strategy()
    {
        foreach(var good in goodslist)
        {
            good.price = good.num*good.price;
        }
        return goodslist;
    }
}
