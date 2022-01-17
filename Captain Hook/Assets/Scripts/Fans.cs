using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fans : MonoBehaviour
{
    public AreaEffector2D areaEffect;

    void Start()
    {
        areaEffect = this.GetComponent<AreaEffector2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            areaEffect.forceMagnitude = 0f;
        } else if(Input.GetKeyUp(KeyCode.S))
        {
            areaEffect.forceMagnitude = 0.02f;
        }
    }
}
