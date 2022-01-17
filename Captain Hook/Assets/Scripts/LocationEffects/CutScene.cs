using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    public bool freeze;
    public Vector2 force;
    public float seconds;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(freeze)
        {
            StartCoroutine("FreezePlayer");
        }
        else
        {
            playerMovement.gameObject.GetComponent<Rigidbody2D>().velocity = force;
        }

    }

    private IEnumerator FreezePlayer()
    {
        playerMovement.SetMasterControl(false);
        playerMovement.gameObject.GetComponent<Rigidbody2D>().velocity = force;
        yield return new WaitForSeconds(seconds);
        playerMovement.SetMasterControl(true);
    }
}
