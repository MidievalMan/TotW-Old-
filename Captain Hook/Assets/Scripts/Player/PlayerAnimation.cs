using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Vector2 velocity;
    private Rigidbody2D rb;

    public PlayerMovement playerMovement;
    public Transform player;
    public Transform hat;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = playerMovement.GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        velocity = rb.velocity;

        // horizontal
        if (velocity.x < -0.6f)
        {
            animator.SetInteger("XDirection", -1);
            hat.position = new Vector3(player.position.x + 0.1f, player.position.y + 0.75f, player.position.z);
            hat.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (velocity.x > 0.6f)
        {
            animator.SetInteger("XDirection", 1);
            hat.position = new Vector3(player.position.x + -0.1f, player.position.y + 0.75f, player.position.z);
            hat.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            animator.SetInteger("XDirection", 0);
            hat.position = new Vector3(player.position.x, player.position.y + 0.75f, player.position.z);
        }

        // vertical
        if (velocity.y > 0.001f)
        {
            animator.SetBool("Grounded", false);
            animator.SetInteger("YDirection", 1);
            hat.position = new Vector3(player.position.x, player.position.y + 0.75f, player.position.z);
        }
        else if (velocity.y < -0.001f)
        {
            animator.SetInteger("YDirection", -1);
            hat.position = new Vector3(player.position.x, player.position.y + 0.75f, player.position.z);
        }
        else
        {
            animator.SetInteger("YDirection", 0);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("WackoDance"))
        {
            animator.speed = 10;
        } else if(collision.gameObject.CompareTag("UnWackoDance"))
        {
            animator.speed = 1;
            animator.SetTrigger("Wacko");
        }
    }
    
   
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            animator.SetBool("Grounded", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            animator.SetBool("Grounded", false);
        }
    }
    
}
