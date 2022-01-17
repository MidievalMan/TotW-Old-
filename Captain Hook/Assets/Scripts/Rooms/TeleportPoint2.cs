using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint2 : MonoBehaviour
{
    public Teleport teleport;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !gameObject.CompareTag("Door"))
        {
            SendMessageUpwards("GoToPoint1");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("Door") && Input.GetKey(KeyCode.W) && Teleport.accessAllowed)
        {
            Teleport.timeSinceThroughDoor = 0f;
            SendMessageUpwards("GoToPoint1");
            SoundManager.PlaySound(SoundManager.Sound.EnterDoor);
        }
    }
}
