using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wacko : MonoBehaviour
{
    public Animator animator;
    public Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("Begin", true);
            playerAnimator.SetTrigger("Wacko");
        }
    }

}
