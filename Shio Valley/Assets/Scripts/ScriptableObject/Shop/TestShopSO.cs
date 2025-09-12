using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Shop/TestShop")]
public class TestShopSO : GeneralShopSO
{
    public override List<Goods> Strategy()
    {
        base.Strategy();
        return goodslist;
    }
}
