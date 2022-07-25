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
    private bool masterControl = true;
    Vector2 input;
    private IMovementState moveState;
    private bool isGrounded;
    

    // Normal Movement Vars
    private float normalSpeed;
    private bool walkControl = true;
    private float easeValue = 1f; // the amount of control of walking player has. Used to ease in and out of inability to move
    public float EaseValue { get => easeValue; set => easeValue = value; }

    private bool isJumping;
    public bool IsJumping { get => isJumping; set => isJumping = value; }
    private float jumpCoolDown = 0.2f;
    public float JumpCoolDown { get => jumpCoolDown; set => jumpCoolDown = value; }
    private float jumpCoolDownTimer;
    public float JumpCoolDownTimer { get => jumpCoolDownTimer; set => jumpCoolDownTimer = value; }
    public float maxJumpTime;
    private float jumpTimeCounter;
    private bool canJump;
    public bool CanJump { get => canJump; set => canJump = value; }
    private bool canRegainJump = true;
    public bool CanRegainJump { get => canRegainJump; set => canRegainJump = value; }
    private bool bufferJump = false;
    private float bufferJumpTime = 0.1f;
    private bool coyoteTimeCoroutineHasNotTriggeredYet;
    private float coyoteTime = 0.05f;
    private bool stopJumpWhenPossible;

    public float dashForce;
    public float dashLength;
    private bool isDashing = false;
    private bool canDash;
    public bool CanDash { get => canDash; set => canDash = value; }
    private float dashRechargeTimer;
    private float dashRechargeTime = 0.4f;
    private bool startDash;
    private Coroutine dashTimerCoroutine = null;
    private bool facingRight = true;

    private bool isCrouching;
    public bool IsCrouching { get => isCrouching; set => isCrouching = value; }
    private float crouchSpeed;

    private bool isRunning;
    private float runSpeed;

    private const float MIN_TO_MAX_JUMP_TIME_RATIO = 0.9f; // Higher value = more control
    private float NORMAL_SPEED_VALUE = 350;

    // Water Movement Vars
    private bool topDownWaterPhysics = true;

    private float dashTimer;
    private float dashTime = 0.5f;

    private float WATER_TO_NORMAL_DRAG_RATIO = 0.75f;
    private float WATER_SPEED_VALUE = 225f;

    // Surface Movement Vars
    private bool startDashSurface;

    private float DASH_FORCE_SURFACE_TO_DASH_FORCE_RATIO = 0.25f;
    //private float SURFACE_TO_NORMAL_DRAG_RATIO = 0.5
    private float SURFACE_SPEED_VALUE = 350f;

    //summons
    public float parachuteModifier = 1f;

    //drag
    private float drag = 0.8f;
    private float slidingDownWallDragFactor = 1f;
    private float gravityStrength = 12f;
    public float GravityStrength { get => gravityStrength; set => gravityStrength = value; }

    private MovementStates state = MovementStates.Normal;

    // Key Codes

    private KeyCode jumpKey = KeyCode.Space;
    private KeyCode dashKey = KeyCode.J;
    public KeyCode summonKey = KeyCode.K;
    private KeyCode interactKey = KeyCode.L;


    private KeyCode upKey = KeyCode.W;
    private KeyCode leftKey = KeyCode.A;
    private KeyCode downKey = KeyCode.S;
    private KeyCode rightKey = KeyCode.D;

    private KeyCode runKey = KeyCode.LeftShift;


    //Dialogue
    [SerializeField] private DialogueUI dialogueUI;

    public DialogueUI DialogueUI => dialogueUI;

    public IInteractable Interactable { get; set; }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = GravityStrength;

        moveState = new LandMoveState();

        distanceGround = col.bounds.extents.y; // for isGrounded

        // Define speeds
        crouchSpeed = speed / 1.5f;
        normalSpeed = speed;
        runSpeed = speed * 1.75f;
    }

    void Update()
    {
        switch(state)
        {
            case MovementStates.Normal:
                ExecuteNormalUpdate();
                break;
            case MovementStates.Water:
                ExecuteWaterUpdate();
                break;
            case MovementStates.Surface:
                ExecuteSurfaceUpdate();
                break;
        }
    }

    void FixedUpdate()
    {
        Debug.Log(moveState);
        switch(state)
        {
            case MovementStates.Normal:
                ExecuteNormalFixed();
                break;
            case MovementStates.Water:
                ExecuteWaterFixedUpdate();
                break;
            case MovementStates.Surface:
                ExecuteSurfaceFixedUpdate();
                break;
        }

        if (startDash)
        {
            StartCoroutine(Dash());
            startDash = false;
        }

        if (startDashSurface)
        {
            StartCoroutine(DashSurface());
            startDashSurface = false;
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

        isGrounded = IsGrounded();

        if(Input.GetKeyDown(interactKey) && !dialogueUI.isOpen)
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
            //masterControl = true; Fix problem wen time comes
        }

        if (masterControl) {

            // Jump
            jumpCoolDownTimer -= Time.deltaTime;
            if (Input.GetKeyDown(jumpKey) && !isDashing && jumpCoolDownTimer < 0) {
                jumpCoolDownTimer = jumpCoolDown;
                StopCoroutine(SetBufferJump());
                StartCoroutine(SetBufferJump());
            }
            if(bufferJump && canJump)
            {
                InitialJump();
                bufferJump = false;
            }
            if (Input.GetKeyUp(jumpKey)) {
                if(jumpTimeCounter < (maxJumpTime * MIN_TO_MAX_JUMP_TIME_RATIO))
                {
                    isJumping = false;
                } else
                {
                    stopJumpWhenPossible = true;
                }

            }
            RaycastHit2D hit1 = Physics2D.Raycast(col.bounds.max, Vector2.up, 0.05f, platformLayerMask);
            //Debug.DrawRay(col.bounds.max, Vector2.up * 0.05f);
            RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(col.bounds.min.x, col.bounds.max.y), Vector2.up, 0.05f, platformLayerMask);
            //Debug.DrawRay(new Vector3(col.bounds.min.x, col.bounds.max.y, 0), Vector2.up * 0.05f);

            if((hit1 && hit1.transform.gameObject.CompareTag("Platforms")) || (hit2 && hit2.transform.gameObject.CompareTag("Platforms")) ) {
            
                isJumping = false;
            }

            RaycastHit2D hitLeftUp = Physics2D.Raycast(new Vector2(col.bounds.min.x, col.bounds.max.y), Vector2.left, 0.05f, platformLayerMask);
            RaycastHit2D hitLeftDown = Physics2D.Raycast(col.bounds.min, Vector2.left, 0.05f, platformLayerMask);
            RaycastHit2D hitRightDown = Physics2D.Raycast(new Vector2(col.bounds.max.x, col.bounds.min.y), Vector2.right, 0.05f, platformLayerMask);
            RaycastHit2D hitRightUp = Physics2D.Raycast(col.bounds.max, Vector2.right, 0.05f, platformLayerMask);

            if(Input.GetKey(leftKey) && (hitLeftUp || hitLeftDown))
            {
                slidingDownWallDragFactor = 5f;
            } else if (Input.GetKey(rightKey) && (hitRightDown || hitRightUp))
            {
                slidingDownWallDragFactor = 5f;
            }
            else
            {
                slidingDownWallDragFactor = 1f;
            }

            // Dash Input
            if (Input.GetKeyDown(dashKey) && canDash) {
                startDash = true;
            }
            // Regain Dash
            if(isGrounded && dashRechargeTimer <= 0) {
                canDash = true;
            }

            // Crouch
            if(Input.GetKey(downKey) && isGrounded) {
                isCrouching = true;
            } else
            {
                isCrouching = false;
            }
            if(isCrouching) {
                speed = crouchSpeed;
                col.size = new Vector2(0.5f, 0.5f);
                col.offset = new Vector3(0f, -0.25f);
            } else { // Optimize?
                col.size = new Vector2(0.5f, 0.9f);
                col.offset = new Vector3(0f, 0f);
            }

            if (Input.GetKeyDown(runKey))
            {
                isRunning = true;
            }
            if (Input.GetKeyUp(runKey))
            {
                isRunning = false;
            }
            if (isRunning)
            {
                speed = runSpeed;
            }

            if(!isRunning && !isCrouching)
            {
                speed = normalSpeed;
            }
        }
    }

    private void ExecuteNormalFixed()
    {
        if(masterControl)
        {
            if (stopJumpWhenPossible || (isJumping && Input.GetKey(jumpKey)))
            {
                Jump();
            }

            dashRechargeTimer -= Time.fixedDeltaTime;

            // Drag
            //Vector2 ve moveState.Drag(rb.velocity);
            ApplyDrag();

            // Horizontal movement when holding left or right
            HorizontalMovement();

        }
    }


    private void ExecuteWaterUpdate()
    {
        

        if (Input.GetKeyDown(jumpKey))
        {
            //maybe store vector in variable instead of using a literal
            rb.AddRelativeForce(new Vector2(0, 100f));
        }

        dashTimer -= Time.deltaTime;
        if (Input.GetKey(dashKey) && dashTimer < 0)
        {
            dashTimer = dashTime;
            startDash = true;
        }

        
    }
    private void ExecuteWaterFixedUpdate()
    {
        ApplyDragWater();
        HorizontalMovement();
        VerticalMovement();
    }

    private void ExecuteSurfaceUpdate()
    {
        if (Input.GetKey(jumpKey))
        {
            rb.AddForce(Vector2.up * 4f, ForceMode2D.Impulse);
            state = MovementStates.Normal;
            gravityStrength = 12;
            rb.gravityScale = gravityStrength;
            speed = NORMAL_SPEED_VALUE;

            canDash = true;
            dashRechargeTimer = 0;
        }

        if (Input.GetKey(downKey))
        {
            state = MovementStates.Water;
            gravityStrength = 0;
            rb.gravityScale = gravityStrength;
            speed = WATER_SPEED_VALUE;
        }

        if (Input.GetKey(dashKey))
        {
            startDashSurface = true;
        }

        dashRechargeTimer -= Time.deltaTime;
    }

    private void ExecuteSurfaceFixedUpdate()
    {
        ApplyDragSurface();
        HorizontalMovement();
    }

    private void ApplyDrag()
    {
        Vector3 velocity = transform.InverseTransformDirection(rb.velocity);
        float force_x = (-drag * EaseValue) * velocity.x;
        float force_y = -drag * 0.5f * velocity.y;
        if (!isDashing)
        {
            if (rb.velocity.y < 0)
            {
                rb.AddRelativeForce(new Vector2(force_x, force_y * slidingDownWallDragFactor * parachuteModifier));
            }
            else
            {
                rb.AddRelativeForce(new Vector2(force_x, 0));
            }
        }
    }

    private void ApplyDragWater()
    {
        Vector3 velocity = transform.InverseTransformDirection(rb.velocity);
        float force_x = (-drag * EaseValue) * velocity.x * WATER_TO_NORMAL_DRAG_RATIO;
        float force_y = (-drag * EaseValue) * velocity.y * WATER_TO_NORMAL_DRAG_RATIO; // adding easeValue untested, if want to use springs underwater, test
        if (!isDashing)
        {
            rb.AddRelativeForce(new Vector2(force_x, force_y * slidingDownWallDragFactor * parachuteModifier));
        }
    }

    private void ApplyDragSurface()
    {
        Vector3 velocity = transform.InverseTransformDirection(rb.velocity);
        float force_x = -drag * velocity.x;// * SURFACE_TO_NORMAL_DRAG_RATIO;
        if (!isDashing)
        {
            rb.AddRelativeForce(new Vector2(force_x, 0));
        }
    }

    private void HorizontalMovement()
    {
        if (walkControl && !isDashing)
        {
            input = new Vector2(Input.GetAxis("Horizontal"), 0) * EaseValue;
            if (Input.GetKey(leftKey))
            {
                rb.AddRelativeForce(Vector2.right * input.x * Time.deltaTime * speed);
                facingRight = false;
            }
            else if (Input.GetKey(rightKey))
            {
                facingRight = true;
                rb.AddRelativeForce(Vector2.right * input.x * Time.deltaTime * speed);
            }
        }
    }

    private void VerticalMovement()
    {
        if (walkControl && !isDashing)
        {
            input = new Vector2(Input.GetAxis("Vertical"), 0) * EaseValue;
            if (Input.GetKey(downKey))
            {
                rb.AddRelativeForce(Vector2.up * input.x * Time.deltaTime * speed);
            }
            else if (Input.GetKey(upKey))
            {
                rb.AddRelativeForce(Vector2.up * input.x * Time.deltaTime * speed);
            }
        }
    }


    #region Dash Logic
    private IEnumerator Dash()
    {
        EventManager.TriggerEvent("dash");

        isDashing = true;
        rb.velocity = new Vector2(0, 0);
        var dashVector = GetDashInput();
        rb.gravityScale = 0f;

        yield return new WaitForSeconds(0.075f);
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(dashVector, ForceMode2D.Impulse);

        isJumping = false;
        canDash = false;

        dashRechargeTimer = dashRechargeTime;
        
        dashTimerCoroutine = StartCoroutine(DashTimer());
    }

    private Vector2 GetDashInput()
    {
        var direction = new Vector2(0, 0);

        if (Input.GetKey(upKey))
        {
            direction += Vector2.up;
        }
        if (Input.GetKey(rightKey))
        {
            direction += Vector2.right;
        }
        if (Input.GetKey(downKey))
        {
            direction += Vector2.down;
        }
        if (Input.GetKey(leftKey))
        {
            direction += Vector2.left;
        }

        direction.Normalize();

        // Default dash direction
        if (direction == Vector2.zero && facingRight)
        {
            direction += Vector2.right;
        } else if (direction == Vector2.zero)
        {
            direction += Vector2.left;
        }

        if(Mathf.Abs(direction.y) > 0) {
            isCrouching = false;
        }

        if (Input.GetKey(upKey) && !Input.GetKey(rightKey) && !Input.GetKey(leftKey))
        {
            return direction * dashForce * 0.9f;
        }
        return direction * dashForce;
    }

    private IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(dashLength);
        isDashing = false;
        rb.gravityScale = GravityStrength;
    }

    private IEnumerator DashSurface()
    {
        EventManager.TriggerEvent("dash");

        isDashing = true;
        rb.velocity = new Vector2(0, 0);
        var dashVector = GetDashInputSurface();

        yield return new WaitForSeconds(0.075f);
        rb.AddForce(dashVector, ForceMode2D.Impulse);

        isJumping = false;
        canDash = false;

        dashRechargeTimer = dashRechargeTime;

        dashTimerCoroutine = StartCoroutine(DashTimerSurface());
    }

    private Vector2 GetDashInputSurface()
    {
        var direction = new Vector2(0, 0);

        if (facingRight)
        {
            direction += Vector2.right;
        }
        else
        {
            direction += Vector2.left;
        }

        return direction * dashForce * DASH_FORCE_SURFACE_TO_DASH_FORCE_RATIO;
    }

    private IEnumerator DashTimerSurface()
    {
        yield return new WaitForSeconds(dashLength);
        isDashing = false;
    }
    #endregion
    #region Jump Logic
    private void InitialJump()
    {
        jumpTimeCounter = maxJumpTime;
        isJumping = true;
        isCrouching = false;

        rb.AddForce(new Vector2(0, jumpForce / 200f), ForceMode2D.Impulse);

        EventManager.TriggerEvent("jump");

        SoundManager.PlaySound(SoundManager.Sound.Jump);
    }

    private void Jump()
    {
        isGrounded = false;
        if(jumpTimeCounter < (maxJumpTime * MIN_TO_MAX_JUMP_TIME_RATIO))
        {
            stopJumpWhenPossible = false;
        }
        if (jumpTimeCounter > 0)
        {
            rb.AddForce(new Vector2(0, jumpForce * Time.fixedDeltaTime), ForceMode2D.Force);
        }
        else
        {
            isJumping = false;
        }
        jumpTimeCounter -= Time.deltaTime;
    }
    #endregion

    /* Cancels all processes related to jumping for the player
     * Things to deal with:
     * buffer jump: set to false
     * reset jump cooldown timer to 0
     * reset jumpTimeCounter, isJumping, and stopJumpWhenPossible
     */
    public void CancelJump()
    {
        StopCoroutine(CoyoteTime());
        StopCoroutine(SetBufferJump());
        bufferJump = false;
        jumpCoolDownTimer = 0;
        jumpTimeCounter = 0;
        isJumping = false;
        stopJumpWhenPossible = false;
    }

    /* Cancels all processes related to dashing for the player
     * canDash = false
     * startDash = false
     * reset dashRechargeTimer to dashRechargeTime
     * stop Dash coroutine
     * isDashing = false
     * stop DashTimer coroutine
     */
    public void CancelDash()
    {
        StopCoroutine(Dash());
        if(dashTimerCoroutine != null) { StopCoroutine(dashTimerCoroutine); }
        canDash = false;
        startDash = false;
        dashRechargeTimer = dashRechargeTime;
        isDashing = false;
        rb.gravityScale = gravityStrength;
    }

    public bool IsGrounded() {
        Vector3 margin = new Vector3(0.2f, 0f, 0f);

        RaycastHit2D hit1 = Physics2D.Raycast(col.bounds.center - margin, Vector2.down, distanceGround + 0.05f, platformLayerMask);
        //Debug.DrawRay(col.bounds.center - margin, Vector2.down * (distanceGround + 0.05f));

        RaycastHit2D hit2 = Physics2D.Raycast(col.bounds.center + margin, Vector2.down, distanceGround + 0.05f, platformLayerMask);
        //Debug.DrawRay(col.bounds.center + margin, Vector2.down * (distanceGround + 0.05f));



        if (hit1.collider || hit2.collider) {
            if(canRegainJump)
            {
                canJump = true;
                coyoteTimeCoroutineHasNotTriggeredYet = true;
                return true;
            }
            
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
            rb.gravityScale = 0;
        } else
        {
            rb.gravityScale = gravityStrength;
        }
    }

    public void SetWalkControl(bool control)
    {
        walkControl = control;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Hazards"))
        {
            StartCoroutine(Death());
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            Instantiate(landingParticleEffect, transform.position, Quaternion.identity);
        } else if (other.CompareTag("Respawn")) {
            PlayerStats.respawnPoint = (Vector2)other.transform.position;
        } else if (topDownWaterPhysics && other.CompareTag("Water"))
        {
            state = MovementStates.Water;
            gravityStrength = 0;
            rb.gravityScale = gravityStrength;
            speed = WATER_SPEED_VALUE;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (topDownWaterPhysics && other.CompareTag("Water"))
        {
            state = MovementStates.Surface;
            rb.velocity = Vector2.zero;
            speed = SURFACE_SPEED_VALUE;
        }
    }

    private IEnumerator Death()
    {
        SetMasterControl(false);

        //animator.SetTrigger("Death");
        //SoundManager.PlaySound(SoundManager.Sound.PlayerDeath);

        yield return new WaitForSeconds(0.5f);

        transform.position = PlayerStats.respawnPoint;
        Debug.Log("transform.position set to " + PlayerStats.respawnPoint);
        
        //animator.SetTrigger("Respawn");
        //SoundManager.PlaySound(SoundManager.Sound.PlayerRespawn);

        yield return new WaitForSeconds(0.5f);

        //hookScript.gameObject.SetActive(true);

        SetMasterControl(true);
    }
}

public enum MovementStates
{
    Normal,
    Water,
    Surface,

}
