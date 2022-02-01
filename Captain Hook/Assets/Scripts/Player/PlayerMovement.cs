using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{

    // References
    public BoxCollider2D col;
    public BoxCollider2D colCrouch;
    private Rigidbody2D rb;
    public Camera mainCam;
    public GrapplingHook hookScript;
    public GameObject landingParticleEffect;

    [SerializeField] private LayerMask platformLayerMask;

    public float jumpForce;
    private float distanceGround;
    public float speed;

    // Private
    private int numJumps;
    private bool masterControl = true;
    private const float WAIT_FOR_COIN = 0.5f;
    private float coinTimer;
    Vector2 input;
    private bool firstPlatformTouch = true;

    // Normal Movement Vars
    private float normalSpeed;

    private bool isJumping;
    public float maxJumpTime;
    private float jumpTimeCounter;
    private bool canJump;
    private bool bufferJump = false;
    private float bufferJumpTime = 0.1f;
    private bool coyoteTimeCoroutineHasNotTriggeredYet;
    private float coyoteTime = 0.06f;

    public float dashForce;
    public float dashLength;
    private Vector2 dashVector;
    private bool isDashing = false;
    private bool canDash;
    private float dashRechargeTimer;
    private float dashRechargeTime = 0.4f;
    private bool startDash = false;

    private bool isCrouching;
    private float crouchSpeed;

    //summons
    public float parachuteModifier = 1f;

    //drag
    public float drag = 1f;
    public float acceleration = 10.0f;

    private MovementState state = MovementState.Normal;

    //Dialogue
    [SerializeField] private DialogueUI dialogueUI;

    public DialogueUI DialogueUI => dialogueUI;

    public IInteractable Interactable { get; set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        distanceGround = col.bounds.extents.y; // for isGrounded
        crouchSpeed = speed / 2f;
        normalSpeed = speed;
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

    private IEnumerator SetBufferJump()
    {
        bufferJump = true;
        yield return new WaitForSeconds(bufferJumpTime);
        bufferJump = false;
    }

    private void ExecuteNormalUpdate()
    {
        if(Input.GetKeyDown(KeyCode.E) && !dialogueUI.isOpen)
        {
            if(Interactable != null)
            {
                Interactable.Interact(this);
            }
        }

        if(dialogueUI.isOpen)
        {
            masterControl = false;
        } else
        {
            masterControl = true;
        }

        if (masterControl) {

            // Jump
            if (Input.GetKeyDown(KeyCode.Space) && !isDashing) {
                StopCoroutine(SetBufferJump());
                StartCoroutine(SetBufferJump());
            }
            if(bufferJump && canJump)
            {
                InitialJump();
                bufferJump = false;
            }
            if (Input.GetKeyUp(KeyCode.Space)) {
                isJumping = false;
            }
            RaycastHit2D hit1 = Physics2D.Raycast(col.bounds.max, Vector2.up, 0.05f, platformLayerMask);
            //Debug.DrawRay(col.bounds.max, Vector2.up * 0.05f);
            RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(col.bounds.min.x, col.bounds.max.y), Vector2.up, 0.05f, platformLayerMask);
            //Debug.DrawRay(new Vector3(col.bounds.min.x, col.bounds.max.y, 0), Vector2.up * 0.05f);
            if (hit1 || hit2) {
                isJumping = false;
            }

            // Dash
            if (Input.GetButton("Fire2") && canDash) {
                startDash = true;
            }
            if(isGrounded() && dashRechargeTimer <= 0) {
                canDash = true;
            }

            // Crouch
            if(Input.GetKeyDown(KeyCode.LeftShift)) {
                isCrouching = true;
            }
            if(Input.GetKeyUp(KeyCode.LeftShift)) {
                isCrouching = false;
            }
            if(isCrouching) {
                speed = crouchSpeed;
                col.size = new Vector2(0.5f, 0.7f);
                col.offset = new Vector3(0f, -0.6f);
            } else {
                speed = normalSpeed;
                col.size = new Vector2(0.5f, 1.7f);
                col.offset = new Vector3(0f, -0.1f);
            }
        }


    }

    private void ExecuteNormalFixed()
    {
        if(masterControl)
        {
            if (isJumping && Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
            if (startDash)
            {
                Dash();
                startDash = false;
            }
            if (isDashing)
            {
                rb.AddForce(dashVector * Time.fixedDeltaTime, ForceMode2D.Force);
            }

            Vector3 velocity = transform.InverseTransformDirection(rb.velocity);
            float force_x = -drag * velocity.x;
            float force_y = -drag * 0.5f * velocity.y;
            if (!isDashing)
            {
                if (rb.velocity.y < 0)
                {
                    Debug.Log("mod: " + parachuteModifier);
                    rb.AddRelativeForce(new Vector2(force_x, force_y * parachuteModifier));
                }
                else
                {
                    rb.AddRelativeForce(new Vector2(force_x, 0));
                }
            }


            if (!isDashing)
            {
                input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    rb.AddRelativeForce(Vector2.right * input.x * Time.deltaTime * speed);
                }
            }

            dashRechargeTimer -= Time.fixedDeltaTime;
        }
    }

    private void Dash()
    {
        dashVector = GetDashInput();
        rb.velocity = dashVector / 40f;
        isDashing = true;
        isJumping = false;
        canDash = false;
        rb.gravityScale = 0f;
        dashRechargeTimer = dashRechargeTime;
        StartCoroutine(DashTimer());
    }

    private Vector2 GetDashInput()
    {
        var direction = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector2.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left;
        }

        direction.Normalize();

        if(Mathf.Abs(direction.y) > 0) {
            isCrouching = false;
        }

        return direction * dashForce;
    }

    private IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(dashLength);
        isDashing = false;
        rb.gravityScale = 15f;
        rb.velocity = dashVector / 25;
    }


    private void InitialJump()
    {
        jumpTimeCounter = maxJumpTime;
        //yPrevious = transform.position.y;
        //yDifference = 0f;
        isJumping = true;
        isCrouching = false;

        rb.AddForce(new Vector2(0, jumpForce / 200f), ForceMode2D.Impulse);

        EventManager.TriggerEvent("jump");

        SoundManager.PlaySound(SoundManager.Sound.Jump);
    }

    private void Jump()
    {
        if (jumpTimeCounter > 0)
        {
            rb.AddForce(new Vector2(0, jumpForce * Time.fixedDeltaTime), ForceMode2D.Force);
            //yDifference += transform.position.y - yPrevious;
            //yPrevious = transform.position.y;
        }
        else
        {
            isJumping = false;
        }
        jumpTimeCounter -= Time.deltaTime;
    }

    private void MoveHorizontal(float percentToMove)
    {
        rb.AddForce(Vector2.right * input.x * Time.deltaTime * speed * percentToMove);
    }



    public bool isGrounded() {
        Vector3 margin = new Vector3(0.2f, 0f, 0f);

        RaycastHit2D hit1 = Physics2D.Raycast(col.bounds.center - margin, Vector2.down, distanceGround + 0.05f, platformLayerMask);
        //Debug.DrawRay(col.bounds.center - margin, Vector2.down * (distanceGround + 0.05f));

        RaycastHit2D hit2 = Physics2D.Raycast(col.bounds.center + margin, Vector2.down, distanceGround + 0.05f, platformLayerMask);
        //Debug.DrawRay(col.bounds.center + margin, Vector2.down * (distanceGround + 0.05f));

        if (hit1.collider || hit2.collider) {
            canJump = true;
            coyoteTimeCoroutineHasNotTriggeredYet = true;
            return true;
        } else if (coyoteTimeCoroutineHasNotTriggeredYet) {
            StartCoroutine(CoyoteTime());
            coyoteTimeCoroutineHasNotTriggeredYet = false;
        }
        return false;
    }

    private IEnumerator CoyoteTime()
    {
        yield return new WaitForSeconds(coyoteTime);
        canJump = false;
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
                mainCam.GetComponent<Follow>().smoothSpeed = 0.25f;
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
