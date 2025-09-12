using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemFollow : MonoBehaviour
{
    public Canvas canvas;
    private void Update()
    {
        Vector2 mousePos = Mouse.current.position.value;
        Vector2 anchoredPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            mousePos,
            null,
            out anchoredPos
        );
        RectTransform tooltip;
        tooltip = gameObject.GetComponent<RectTransform>();
        tooltip.anchoredPosition = anchoredPos;
    }
}
