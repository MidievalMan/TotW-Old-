using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform point1;
    private Vector3 point1Pos;
    public Transform point2;
    private Vector3 point2Pos;

    public BoxCollider2D room1Col;
    public BoxCollider2D room2Col;

    private GameObject mainCam;
    private Transform mainCamPos;

    private GameObject player;
    private PlayerMovement playerMovement;
    private Transform playerPos;

    private static float doorWaitTime = 10f;
    public static float timeSinceThroughDoor = 0f;
    public static bool accessAllowed;

    private void Start()
    {
        mainCam = GameObject.Find("Main Camera");
        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();

        playerPos = player.transform;
        mainCamPos = mainCam.transform;

        point1Pos = point1.position;
        point2Pos = point2.position;
    }
    public void GoToPoint1()
    {
        playerPos.position = point1Pos;
        mainCamPos.position = point1Pos;
        PlayerStats.respawnPoint = point1Pos;

    }
    public void GoToPoint2()
    {
        playerPos.position = point2Pos;
        mainCamPos.position = point2Pos;
        PlayerStats.respawnPoint = point2Pos;

    }

    private void Update()
    {
        if (timeSinceThroughDoor >= doorWaitTime)
        {
            accessAllowed = true;
        }
        else
        {
            accessAllowed = false;
        }
    }

    private void FixedUpdate()
    {
        timeSinceThroughDoor += 0.02f;
    }
}
