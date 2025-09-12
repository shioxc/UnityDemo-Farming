using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestShopButton : MonoBehaviour,IPointerClickHandler
{
    public GeneralShopSO shopSO;
    public GoodsListEventSO OpenShopSO;
    public void OnPointerClick(PointerEventData eventData)
    {
        OpenShopSO.RaiseEvent(shopSO.Strategy());
    }
}
