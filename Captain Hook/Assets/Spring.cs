using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private float force = 60f;
    public Direction direction;

    private bool readyForPlayer = true;
    private float readyCountdown = 0f;
    private float interval = 0.1f;

    Rigidbody2D rb;
    PlayerMovement player;
    GrapplingHook hook;
    BoxCollider2D box;
    Animator animator;

    void Start()
    {
        rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        hook = rb.gameObject.transform.GetChild(0).gameObject.GetComponent<GrapplingHook>();
        box = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        if (direction == Direction.Up)
        {
            box.offset = new Vector2(0f, -0.375f);
            box.size = new Vector2(1f, 0.25f);

            animator.SetBool("up", true);
        }
        else if (direction == Direction.Right)
        {
            box.offset = new Vector2(-0.375f, 0f);
            box.size = new Vector2(0.25f, 1f);

            animator.SetBool("right", true);
        }
        else if (direction == Direction.Down)
        {
            box.offset = new Vector2(0f, 0.375f);
            box.size = new Vector2(1f, 0.25f);

            animator.SetBool("down", true);
        }
        else if (direction == Direction.Left)
        {
            box.offset = new Vector2(0.375f, 0f);
            box.size = new Vector2(0.25f, 1f);

            animator.SetBool("left", true);
        }

        readyCountdown = interval;
    }

    void Update()
    {
        // keep player from hitting spring multiple times on one run through
        if(!readyForPlayer)
        {
            readyCountdown -= Time.deltaTime;

            if(readyCountdown < 0)
            {
                readyForPlayer = true;

                readyCountdown = interval;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && readyForPlayer)
        {
            hook.StopAllCoroutines();
            hook.ReachedTarget();

            hook.grappleLine.gameObject.SetActive(false);
            player.SetIsJumping(false);
            

            if(direction == Direction.Up)
            {
                rb.velocity = new Vector2(rb.velocity.x, force * 0.8f);
            }
            else if(direction == Direction.Right)
            {
                rb.velocity = new Vector2(force, rb.velocity.y);
            }
            else if (direction == Direction.Down)
            {
                rb.velocity = new Vector2(rb.velocity.x, -force * 0.6f);
            }
            else if (direction == Direction.Left)
            {
                rb.velocity = new Vector2(-force, rb.velocity.y);
            }

            SoundManager.PlaySound(SoundManager.Sound.Spring);
            animator.SetTrigger("launch");

            readyForPlayer = false;
        }
    }

}

public enum Direction
{
    Up,
    Right,
    Down,
    Left,
}
