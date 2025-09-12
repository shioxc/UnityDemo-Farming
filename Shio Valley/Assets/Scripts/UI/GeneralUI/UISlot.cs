using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlot : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler,IPointerExitHandler
{
    public GameObject curUI;
    public GameobjectEventSO ChangeUIEventSO;
    public Image icon;
    public void OnPointerClick(PointerEventData eventData)
    {
        ChangeUIEventSO.RaiseEvent(curUI);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        icon.color = new Color(0f, 0f, 0f, 0.5f);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        icon.color = new Color(0f, 0f, 0f, 0f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        icon.color = new Color(0f, 0f, 0f, 0f);
    }
}
