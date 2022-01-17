using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marty : MonoBehaviour
{

    private CircleCollider2D col;
    private Animator animator;
    private bool hasSetPop = false;

    void Start()
    {
        col = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        hasSetPop = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!hasSetPop && collision.CompareTag("Player"))
        {
            animator.SetTrigger("Pop");
            hasSetPop = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!hasSetPop && collision.CompareTag("Player"))
        {
            animator.SetTrigger("Pop");
            hasSetPop = true;
        }
    }
}
