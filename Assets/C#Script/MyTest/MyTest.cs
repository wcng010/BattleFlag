using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTest : MonoBehaviour
{
    private Animator myAnimator;
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            myAnimator.SetBool("Move",true);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            myAnimator.SetBool("Move",false);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            myAnimator.SetTrigger("Attack");
        }
    }
}
