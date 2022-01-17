using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnPickupThenDestroy : MonoBehaviour
{
    AudioSource audioSource;

    private float timer = 0;
    private bool startTimer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(startTimer)
        {
            timer += Time.deltaTime;
            if(timer > audioSource.clip.length)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(audioSource.clip);
            startTimer = true;
        }
    }
}
