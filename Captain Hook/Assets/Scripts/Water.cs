using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public AreaEffector2D areaEffect;
    public float forceMagnitude;
    public float crouchingforceMagnitude;
    private float regularDrag = 1f;
    //public float downwardDrag;

    void Start()
    {
        areaEffect = this.GetComponent<AreaEffector2D>();
        areaEffect.forceMagnitude = forceMagnitude;
        areaEffect.drag = regularDrag;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            areaEffect.forceMagnitude = crouchingforceMagnitude;
            //areaEffect.drag = downwardDrag;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            areaEffect.forceMagnitude = forceMagnitude;
            //areaEffect.drag = regularDrag;
        }
    }
}
