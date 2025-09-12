using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Shop/GoodsListEventSO")]
public class GoodsListEventSO : ScriptableObject
{
    public UnityAction<List<Goods>> OnEventRaised;
    public void RaiseEvent(List<Goods> goods)
    {
        OnEventRaised?.Invoke(goods);
    }
}
