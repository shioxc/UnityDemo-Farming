using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandSelectSlot : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    private GameObject hand;
    private void Awake()
    {
        hand = transform.parent.gameObject;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        hand.GetComponent<HandSelectManager>()?.SelectSlot(gameObject);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        hand.GetComponent<HandSelectManager>()?.CheckProfile(gameObject);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        hand.GetComponent<HandSelectManager>()?.DisableProfile();
    }
}
