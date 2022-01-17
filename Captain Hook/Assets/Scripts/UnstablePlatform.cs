using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstablePlatform : MonoBehaviour
{

    private Animator animator;
    private BoxCollider2D box;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0;

        box = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {

        if(col.gameObject.CompareTag("Player"))
        {
            animator.speed = 1;
            
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {

        if (col.gameObject.CompareTag("Player"))
        {
            animator.speed = 1;

        }
    }

    public void Disable()
    {
        Debug.Log("Triggered");
        box.enabled = false;
        animator.speed = 0;
        StartCoroutine("ResetBlock");
    }

    public void ResetSpeed()
    {
        animator.speed = 0;
    }
    
    private IEnumerator ResetBlock()
    {
        yield return new WaitForSeconds(5f);
        animator.speed = 1;
        box.enabled = true;
        animator.SetTrigger("Crumble");
    }
    

}
