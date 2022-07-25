using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parachute : MonoBehaviour
{
    [SerializeField] private PlayerMovement pm;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(pm.summonKey)/*Input.GetButtonDown("Fire1")*/)
        {
            DeployParachute();
        }
        if (Input.GetKeyUp(pm.summonKey)/*Input.GetButtonUp("Fire1") */)
        {
            RetractParachute();
        }
    }

    private void DeployParachute()
    {
        spriteRenderer.enabled = true;
        pm.parachuteModifier = 4f;
    }

    private void RetractParachute()
    {
        spriteRenderer.enabled = false;
        pm.parachuteModifier = 1f;
    }
}
