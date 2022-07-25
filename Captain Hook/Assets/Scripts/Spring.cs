using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private float force = 25;
    public Direction direction;

    private bool readyForPlayer = true;
    private float readyCountdown = 0f;
    private float interval = 0.1f;

    private float lerpTimer;
    private float easeTime = 0.5f;

    private bool startRegainJumpTimer;
    private float RegainJumpTimer;
    private float RegainJumpTime = 0.25f;

    private float timeHorizontalMovementDisabled = 0.25f;

    // Coroutine variables
    //public static Coroutine lowerGravity;
    //public Coroutine LowerGravity { get => lowerGravity; set => lowerGravity = value; }
    //private static Coroutine disableHorizontalMovement;

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

        EventManager.StartListening("dash", CancelSpringCoroutinesDash);
        EventManager.StartListening("spring", CancelSpringCoroutinesSpring);

        if (direction == Direction.Up)
        {
            box.offset = new Vector2(0f, -0.25f);
            box.size = new Vector2(1f, 0.5f);

            animator.SetBool("up", true);
        }
        else if (direction == Direction.Right)
        {
            box.offset = new Vector2(-0.25f, 0f);
            box.size = new Vector2(0.5f, 1f);

            animator.SetBool("right", true);
        }
        else if (direction == Direction.Down)
        {
            box.offset = new Vector2(0f, 0.25f);
            box.size = new Vector2(1f, 0.5f);

            animator.SetBool("down", true);
        }
        else if (direction == Direction.Left)
        {
            box.offset = new Vector2(0.25f, 0f);
            box.size = new Vector2(0.5f, 1f);

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

        // Lerped horizontal movement to ease player back into movement
        if (lerpTimer <= easeTime)
        {
            player.EaseValue = Mathf.Lerp(0f, 1f, lerpTimer / easeTime);
            lerpTimer += Time.deltaTime;
        }


        if (startRegainJumpTimer)
        {
            RegainJumpTimer += Time.deltaTime;
        }
        if (RegainJumpTimer > RegainJumpTime)
        {
            startRegainJumpTimer = false;
            RegainJumpTimer = 0;
            player.CanRegainJump = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && readyForPlayer)
        {

            player.CancelDash();
            player.CancelJump();

            player.CanDash = true;
            player.IsCrouching = false;

            
            if(!(direction == Direction.Down))
            {
                player.CanRegainJump = false;
                startRegainJumpTimer = true;
                player.CanJump = false;
            }

            if (direction == Direction.Up)
            {
                rb.velocity = new Vector2(rb.velocity.x, force * 1.2f);
                rb.gravityScale = player.GravityStrength;

                StartCoroutine(LowerGravity(0.3f, 0.6f));
            }
            else if(direction == Direction.Right)
            {
                EventManager.TriggerEvent("spring");

                rb.velocity = new Vector2(force * 0.75f, force * 0.6f);

                player.SetWalkControl(true);
                lerpTimer = 1f; // Reset lerp timer to ease player into movement

                rb.gravityScale = player.GravityStrength;

                StartCoroutine(DisableHorizontalMovement(timeHorizontalMovementDisabled));
                StartCoroutine(LowerGravity(0.3f, 0.5f));
            }
            else if (direction == Direction.Down)
            {
                rb.velocity = new Vector2(rb.velocity.x, -force);
            }
            else if (direction == Direction.Left)
            {
                EventManager.TriggerEvent("spring");

                rb.velocity = new Vector2(-force * 0.75f, force * 0.6f);

                player.SetWalkControl(true);
                lerpTimer = 1f;

                rb.gravityScale = player.GravityStrength;

                StartCoroutine(DisableHorizontalMovement(timeHorizontalMovementDisabled));
                StartCoroutine(LowerGravity(0.3f, 0.5f));
            }

            SoundManager.PlaySound(SoundManager.Sound.Spring);
            animator.SetTrigger("launch");

            readyForPlayer = false;
        }
    }

    public IEnumerator LowerGravity(float gravityMultiplier, float lowerTimeInSeconds)
    {
        rb.gravityScale = player.GravityStrength * gravityMultiplier;
        yield return new WaitForSeconds(lowerTimeInSeconds);
        rb.gravityScale = player.GravityStrength;
    }

    public IEnumerator DisableHorizontalMovement(float disableTimeInSeconds)
    {
        player.SetWalkControl(false);
        player.EaseValue = 0f;
        yield return new WaitForSeconds(disableTimeInSeconds);
        player.SetWalkControl(true);
        lerpTimer = 0f; // Reset lerp timer to ease player into movement
    }

    private void CancelSpringCoroutinesSpring()
    {
        StopAllCoroutines();

        // For LowerGravity
        rb.gravityScale = player.GravityStrength;

        // For DisableHorizontalMovement
        player.SetWalkControl(true);
        lerpTimer = 1f;
        player.EaseValue = 0f; // No movement
    }

    private void CancelSpringCoroutinesDash()
    {
        StopAllCoroutines();

        // For LowerGravity
        rb.gravityScale = player.GravityStrength;

        // For DisableHorizontalMovement
        player.SetWalkControl(true);
        lerpTimer = 1f;
        player.EaseValue = 1f; // Full movement
    }

}


public enum Direction
{
    Up,
    Right,
    Down,
    Left,
}
