using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceChecker : MonoBehaviour
{
    public BoundEnum boundEnum;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.parent == null)
        {
            Debug.LogWarning("´í");
            return;
        }
        collision.transform.parent.parent.GetComponent<Character>()?.ChangeLayer(boundEnum);
    }
}
