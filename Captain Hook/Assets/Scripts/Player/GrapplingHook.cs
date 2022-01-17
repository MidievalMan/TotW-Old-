using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GrapplingHook : MonoBehaviour {
    // Consts
    const float DISTANCE_FROM_GRAPPLE_TARGET_TO_STOP = 0.5f;
    const float DISTANCE_FROM_TARGET = 0.25f;
    const float MIN_TIME_BETWEEN_GRAPPLES = 0.33f;

    // Calculation/misc vars
    [SerializeField] private LayerMask platformLayerMask;
    public Vector3 lookDirection;
    public bool isNotGrappling = true;
    public bool canGrapple = true;
    public RaycastHit2D hit;
    public RaycastHit2D grappleHit;
    public bool grapplingLineHitTarget;
    private float grappleTimer = 0f;
    public bool usePullHook = true;
    public float pushForce;


    // Line Renderers
    public LineRenderer grappleLine;
    public Gradient gradientForGrappleLine;

    public LineRenderer aimLine;
    public Gradient gradientForAimLine1;
    public Gradient gradientForAimLine2;

    // Refs to player
    public PlayerMovement playerMovement;
    public Transform player;
    private Rigidbody2D rb;

    // Stats
    private float speed;
    private int maxReach;


    private void Start() {
        speed = PlayerStats.hookSpeed;
        maxReach = PlayerStats.hookMaxReach;

        rb = playerMovement.GetComponent<Rigidbody2D>();
        grappleLine.colorGradient = gradientForGrappleLine;
        aimLine.colorGradient = gradientForAimLine1;

        aimLine.gameObject.SetActive(false);
    }

    void Update() {
        if(playerMovement.GetMasterControl()) {
            if(PlayerStats.pullHookUnlocked) {
                aimLine.gameObject.SetActive(true);
            }
            
            if (Input.GetMouseButtonDown(1) && PlayerStats.pushHookUnlocked) {
                usePullHook = !usePullHook;
                if (aimLine.colorGradient.Evaluate(0f) == gradientForAimLine1.Evaluate(0f)) {
                    aimLine.colorGradient = gradientForAimLine2;
                } else {
                    aimLine.colorGradient = gradientForAimLine1;
                }
            }

            grappleTimer -= Time.deltaTime;

            hit = Raycast();
            AimGrapplingHook();

            if (PlayerStats.pullHookUnlocked &&
                Input.GetMouseButtonDown(0) &&
                hit &&
                Vector3.Distance(hit.point, player.position) < maxReach &&
                canGrapple &&
                grappleTimer < 0 &&
                isNotGrappling &&
                !EventSystem.current.IsPointerOverGameObject()) {

                grappleHit = Raycast(); // Get info on what hit
                if (!grappleHit.transform.gameObject.CompareTag("Ungrappleable")) {
                    Grapple(grappleHit);
                    grappleTimer = MIN_TIME_BETWEEN_GRAPPLES;
                }
            }

            if (grapplingLineHitTarget) {

                StartCoroutine("PullPlayerInTowardsHitPoint", grappleHit);
                grapplingLineHitTarget = false;
            }
        } else {
            aimLine.gameObject.SetActive(false);
        }
    }

    public RaycastHit2D Raycast() {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2);
        lookDirection = Camera.main.ScreenToWorldPoint(mousePos);
        return Physics2D.Raycast(player.position, lookDirection - player.position, maxReach, platformLayerMask);
    }

    private void AimGrapplingHook() {
        Vector3 mousePos = lookDirection - player.position;
        aimLine.SetPosition(0, new Vector3(0, 0, 0));
        if (IsMouseWithinMaxReach()) {
            aimLine.SetPosition(1, mousePos);
        } else {
            aimLine.SetPosition(1, mousePos.normalized * maxReach);
        }

        grappleLine.SetPosition(0, player.position);
    }

    private bool IsMouseWithinMaxReach() {
        if (Vector3.Distance(lookDirection, player.position) <= maxReach) { return true; }
        else { return false; }
    }

    private void Grapple(RaycastHit2D hit) {
        PrepareGrapple();

        LaunchGrapple(hit);

    }

    private void PrepareGrapple() {
        if(usePullHook) {
            //playerMovement.canMove = false;
        }


        grappleLine.SetPosition(1, player.position);
        SoundManager.PlaySound(SoundManager.Sound.GrappleNormal);

        grappleLine.gameObject.SetActive(true);
        aimLine.gameObject.SetActive(false);
    }

    private void LaunchGrapple(RaycastHit2D hit) {
        isNotGrappling = false;
        StopCoroutine("ShootGrapplingHookTowardsHitPoint");
        StartCoroutine("ShootGrapplingHookTowardsHitPoint", hit);
    }

    private IEnumerator ShootGrapplingHookTowardsHitPoint(RaycastHit2D hit) {
        if(!usePullHook) {
            
            while (Vector2.Distance(grappleLine.GetPosition(1), hit.point) > DISTANCE_FROM_TARGET) {
                grappleLine.SetPosition(1, Vector2.MoveTowards(grappleLine.GetPosition(1), hit.point, speed * Time.deltaTime * 2));
                grappleLine.SetPosition(0, player.position);
                yield return null;
            }
            grappleLine.SetPosition(1, hit.point);
            grapplingLineHitTarget = true;

        } else {
            grappleLine.SetPosition(0, player.position);
            grappleLine.SetPosition(1, hit.point);
            grapplingLineHitTarget = true;
        }
    }

    private IEnumerator PullPlayerInTowardsHitPoint(RaycastHit2D hit) {
        Vector3 mousePos = lookDirection - player.position;
        if (usePullHook) {
            while (Vector2.Distance(player.position, hit.point) > DISTANCE_FROM_GRAPPLE_TARGET_TO_STOP) {
                player.position = Vector2.MoveTowards(player.position, hit.point, speed * Time.deltaTime);
                grappleLine.SetPosition(0, player.position);
                yield return null;
            }

            if (Vector2.Distance(player.position, hit.point) <= DISTANCE_FROM_GRAPPLE_TARGET_TO_STOP) {
                ReachedTarget();
            }
        } else {
            rb.AddForce(-mousePos.normalized * pushForce, ForceMode2D.Force);
            aimLine.gameObject.SetActive(true);
            grappleLine.gameObject.SetActive(false);
            isNotGrappling = true;
        }

    }

    // copied from playermovement, doesn't work
    public void Drop() {
        grappleLine.gameObject.SetActive(false);
    }

            /*if (Input.GetKeyDown(KeyCode.LeftShift) && hookScript.isNotGrappling)
            {
                dropNextFixed = true;
            }*/

    // currently unused
    public IEnumerator RetractHook() {
        StopCoroutine("ShootGrapplingHookTowardsHitPoint");
        StopCoroutine("PullPlayerInTowardsHitPoint");

        while (Vector2.Distance(grappleLine.GetPosition(1), player.position) > 0.01f) {
            grappleLine.SetPosition(1, Vector2.MoveTowards(grappleLine.GetPosition(1), player.position, speed * Time.deltaTime));
            yield return null;
        }
        grappleLine.gameObject.SetActive(false);
        aimLine.gameObject.SetActive(true);
    }

    // currently unused
    public void RetractHookFunction() {
        StartCoroutine("RetractHook");
    }

    public void ReachedTarget() {
        aimLine.gameObject.SetActive(true);
        isNotGrappling = true;
    }
}
