using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEnvironment : MonoBehaviour
{
    public EnvironmentEffects environment;
    public Area place;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            environment.area = place;
        }
    }
}
