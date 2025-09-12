using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;
    private GameObject curCollider;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        curCollider = transform.Find("Colliders/Layer1").gameObject;//¡Ÿ ±
        curHealth = maxHealth;//¡Ÿ ±
    }

    public void ChangeLayer(BoundEnum boundState)
    {
        curCollider.SetActive(false);

        switch (boundState)
        {
            case BoundEnum.LayerBound1:
                {
                    GameObject obj = transform.Find("Colliders/Layer1").gameObject;
                    obj.SetActive(true);
                    curCollider = obj;
                    break;
                }
            case BoundEnum.LayerBound2:
                {
                    GameObject obj = transform.Find("Colliders/Layer2").gameObject;
                    obj.SetActive(true);
                    curCollider = obj;
                    break;
                }
        }
    }
}
