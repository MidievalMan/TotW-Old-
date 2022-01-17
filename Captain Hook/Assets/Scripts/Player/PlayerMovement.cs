using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{

    // References
    BoxCollider2D col;
    private Rigidbody2D rb;
    public Camera mainCam;

    [SerializeField] private LayerMask platformLayerMask;

    // Public


    public float jumpForce;

    private float distanceGround;

    public float gravity;
    public float speed;

    private const float MAX_HORIZONTAL_SPEED = 10.5f;
    private const float MAX_VERTICAL_SPEED = 30;

    public GrapplingHook hookScript;

    private float velCapY;
    public GameObject landingParticleEffect;

    // Private


    private int numJumps;
    private bool masterControl = true;
    private const float WAIT_FOR_COIN = 0.5f;
    private float coinTimer;
    Vector2 input;
    private bool firstPlatformTouch = true;

    private bool isJumping;
    public float yGoal;
    private float yPrevious;
    private float yDifference;
    public float maxJumpTime;
    private float jumpTimeCounter;

    private const float NORMAL_H_DRAG = 0.75f;
    private const float SPEEDY_H_DRAG = 0.95f;
    public float horizontalDrag;
    private float velocityStep;
    private float velocityCapStrength = 0.5f;

    private MovementState state = MovementState.Normal;

    private void Awake()
    {
        //SoundManager.Initialize();
    }

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        distanceGround = col.bounds.extents.y; // for isGrounded
        horizontalDrag = NORMAL_H_DRAG;
    }

    void Update()
    {
        switch(state)
        {
            case MovementState.Normal:
                ExecuteNormalUpdate();
                break;
        }

    }

    void FixedUpdate()
    {
        switch(state)
        {
            case MovementState.Normal:
                ExecuteNormalFixed();
                break;
        }

    }

    private void ExecuteNormalUpdate()
    {

        if (masterControl) {
            // jumping
            if (Input.GetKeyDown(KeyCode.Space) && hookScript.isNotGrappling && isGrounded()) {

                yPrevious = transform.position.y;
                yDifference = 0f;
                isJumping = true;
                jumpTimeCounter = maxJumpTime;

                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                EventManager.TriggerEvent("jump");

                SoundManager.PlaySound(SoundManager.Sound.Jump);
            }

            if (isJumping && Input.GetKey(KeyCode.Space)) {
                if (jumpTimeCounter > 0 && yDifference < yGoal) {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    jumpTimeCounter -= Time.deltaTime;

                    yDifference += transform.position.y - yPrevious;
                    yPrevious = transform.position.y;
                } else {
                    isJumping = false;
                }

            }

            if (Input.GetKeyUp(KeyCode.Space)) {
                isJumping = false;
            }

            input = new Vector2(Input.GetAxis("Horizontal"), 0);
        }
    }

    private void ExecuteNormalFixed() {
        if (masterControl) {

            rb.velocity = new Vector2(rb.velocity.x * horizontalDrag, rb.velocity.y);
            // Horizontal Movement
            if (rb.velocity.x > MAX_HORIZONTAL_SPEED + MAX_HORIZONTAL_SPEED / 3f) {
                horizontalDrag = SPEEDY_H_DRAG;
                velocityStep += Time.deltaTime * 0.1f;// (1f / velocityCapStrength);
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, MAX_HORIZONTAL_SPEED, velocityStep), rb.velocity.y);
            } else if (rb.velocity.x < -MAX_HORIZONTAL_SPEED - MAX_HORIZONTAL_SPEED / 3f) {
                horizontalDrag = SPEEDY_H_DRAG;
                velocityStep += Time.deltaTime * 0.1f;// (1f / velocityCapStrength);
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, MAX_HORIZONTAL_SPEED, velocityStep), rb.velocity.y);
            } else {
                horizontalDrag = NORMAL_H_DRAG;
                velocityStep = 0f;
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
                MoveHorizontal();
            }

            // Gravity
            if (rb.velocity.y > -MAX_VERTICAL_SPEED) {
                rb.AddForce(new Vector2(0, -gravity) * Time.deltaTime, ForceMode2D.Force);
            }

            coinTimer += 0.02f;
        }
    }

    public bool isGrounded() {
        Vector3 margin = new Vector3(0.4f, 0f, 0f);
        RaycastHit2D hit1 = Physics2D.Raycast(col.bounds.center - margin, Vector2.down, distanceGround + 0.05f, platformLayerMask);;
        //Debug.DrawRay(col.bounds.center - margin, Vector2.down * (distanceGround + 0.05f));

        RaycastHit2D hit2 = Physics2D.Raycast(col.bounds.center + margin, Vector2.down, distanceGround + 0.05f, platformLayerMask); ;
        //Debug.DrawRay(col.bounds.center + margin, Vector2.down * (distanceGround + 0.05f));
        if (hit1.collider || hit2.collider) {
            return true;
        }
        return false;
    }

    public void MoveHorizontal() {
        
        if (Mathf.Abs(rb.velocity.x) > MAX_HORIZONTAL_SPEED) {
            input.x = 0;
        }

        // turn around fast
        if (Input.GetKey(KeyCode.A) && rb.velocity.x > 0) {
            rb.velocity = new Vector2(rb.velocity.x * 0.9f, rb.velocity.y);
        } else if (Input.GetKey(KeyCode.D) && rb.velocity.x < 0) {
            rb.velocity = new Vector2(rb.velocity.x * 0.9f, rb.velocity.y);
        }

        rb.AddForce(Vector2.right * input.x * Time.deltaTime * speed);
    }

    public bool GetMasterControl() {
        return masterControl;
    }

    public void SetMasterControl(bool control) {
        masterControl = control;
        if(!control)
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazards"))
        {
            Death();
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            SoundManager.PlaySound(SoundManager.Sound.Wall);
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("CheckPoint"))
        {
            PlayerStats.respawnPoint = (Vector2)collision.transform.position;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            if(firstPlatformTouch) // for first cutscene
            {
                mainCam.GetComponent<Follow>().smoothSpeed = 0.125f;
                firstPlatformTouch = false;
            }

            Instantiate(landingParticleEffect, transform.position, Quaternion.identity);
        }
    }

    private IEnumerator Death()
    {
        SetMasterControl(false);
        //animator.SetTrigger("Death");

        hookScript.StopAllCoroutines();
        hookScript.gameObject.SetActive(false);

        SoundManager.PlaySound(SoundManager.Sound.PlayerDeath);

        yield return new WaitForSeconds(0.5f);

        transform.position = PlayerStats.respawnPoint;
        //animator.SetTrigger("Respawn");
        //SoundManager.PlaySound(SoundManager.Sound.PlayerRespawn);

        yield return new WaitForSeconds(0.5f);

        hookScript.gameObject.SetActive(true);

        SetMasterControl(true);
    }

    public void SetIsJumping(bool isJumping)
    {
        this.isJumping = isJumping;
    }
}

public enum MovementState
{
    Normal,
}
