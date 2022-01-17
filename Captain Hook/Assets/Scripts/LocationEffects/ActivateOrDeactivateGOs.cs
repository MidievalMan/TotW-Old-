using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOrDeactivateGOs : MonoBehaviour
{
    public GameObject[] gameObjectsToActivate;
    public GameObject[] gameObjectsToDeactivate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            for (int i = 0; i < gameObjectsToActivate.Length; i++)
            {
                gameObjectsToActivate[i].SetActive(true);
            }
            for (int i = 0; i < gameObjectsToDeactivate.Length; i++)
            {
                gameObjectsToDeactivate[i].SetActive(false);
            }
        }
        
    }

}
