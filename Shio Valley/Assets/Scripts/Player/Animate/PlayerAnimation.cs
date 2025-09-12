using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        SetAnimation();
    }
    private void SetAnimation()
    {
        if (rb.velocity.x != 0 || rb.velocity.y != 0)
        {
            anim.SetBool("walking", true);
        }
        else
        {
            anim.SetBool("walking", false);
        }
    }
    public void SetUseAnimation(string item)
    {
        anim.SetTrigger(item);
    }
}
