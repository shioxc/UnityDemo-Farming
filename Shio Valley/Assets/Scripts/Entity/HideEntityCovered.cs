using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideEntityCovered : MonoBehaviour
{
    public SpriteRenderer sr;
    public void FadeIn()
    {
        sr.color = new Color(1f, 1f, 1f, 0.6f);
    }
    public void FadeOut()
    {
        sr.color = new Color(1f, 1f, 1f, 1f);
    }
}
