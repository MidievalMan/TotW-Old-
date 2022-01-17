using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResetMovingPlatform : MonoBehaviour
{
    public GameObject pointOfRespawn;
    public GameObject platform;

    public TMP_Text text;

    private const float timerTime = 0.5f;
    private float timer;

    private void FixedUpdate()
    {
        timer += 0.02f;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text.gameObject.SetActive(true);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.W) && collision.CompareTag("Player") && timer > timerTime)
        {
            timer = 0;
            //SoundManager.PlaySound(SoundManager.Sound.Switch, 0.5f);
            platform.transform.position = pointOfRespawn.transform.position;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text.gameObject.SetActive(false);
        }

    }

}
