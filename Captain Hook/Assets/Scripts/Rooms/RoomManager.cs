using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject virtualCam;
    private Transform playerTransform;

    public GameObject objects;
    public GameObject triggers;
    public GameObject lights;
    public GameObject respawns;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        objects = transform.GetChild(0).gameObject;
        triggers = transform.GetChild(1).gameObject;
        lights = transform.GetChild(2).gameObject;
        respawns = transform.GetChild(3).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(true);

            //PlayerStats.respawnPoint = new Vector2(playerTransform.position.x, playerTransform.position.y);

            objects.SetActive(true);
            triggers.SetActive(true);
            lights.SetActive(true);
            respawns.SetActive(true);
        }


    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(false);

            StartCoroutine(DeactivateChildrenAfterDelay());
        }
    }

    private IEnumerator DeactivateChildrenAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);

        objects.SetActive(false);
        triggers.SetActive(false);
        lights.SetActive(false);
        respawns.SetActive(false);
    }
}
