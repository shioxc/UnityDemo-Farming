using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCover : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        bool flag = false;
        if(other is BoxCollider2D box)
        {
            Transform trans = other.transform;
            if(trans.position.y + box.offset.y - box.size.y/2 <= transform.position.y )
                flag = true;
        }
        if (flag)
        {
            other.GetComponent<HideEntityCovered>()?.FadeIn();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        other.GetComponent <HideEntityCovered>()?.FadeOut();
    }
}
