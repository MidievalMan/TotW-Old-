using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamPos : MonoBehaviour
{
    public Transform mainCam;
    public float shiftX;
    public float shiftY;
    private Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            mainCam.position = player.position + new Vector3(shiftX, shiftY, -5);
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        mainCam.position = player.position;
    }


}
