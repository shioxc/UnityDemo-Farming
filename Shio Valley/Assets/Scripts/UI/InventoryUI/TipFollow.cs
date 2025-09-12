using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TipFollow : MonoBehaviour
{
    public Canvas canvas;
    private Vector2 offset = new Vector2(5, 5);
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
        

        float halfWidth = Screen.width / 2f;
        float halfHeight = Screen.height / 2f;

        if (mousePos.x < halfWidth && mousePos.y > halfHeight)
        {
            tooltip.pivot = new Vector2(0, 1);
            anchoredPos += new Vector2(offset.x, -offset.y);
        }
        else if (mousePos.x >= halfWidth && mousePos.y > halfHeight)
        {
            tooltip.pivot = new Vector2(1, 1);
            anchoredPos += new Vector2(-offset.x, -offset.y);
        }
        else if (mousePos.x < halfWidth && mousePos.y <= halfHeight)
        {
            tooltip.pivot = new Vector2(0, 0);
            anchoredPos += new Vector2(offset.x, offset.y);
        }
        else
        {
            tooltip.pivot = new Vector2(1, 0);
            anchoredPos += new Vector2(-offset.x, offset.y);
        }
        tooltip.anchoredPosition = anchoredPos;
    }
}
