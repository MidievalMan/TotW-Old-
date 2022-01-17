using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemEditor : MonoBehaviour
{

    public ParticleSystem ps;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            var em = ps.emission;
            em.enabled = false;
        }

    }
}
