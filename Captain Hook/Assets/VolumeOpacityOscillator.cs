using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class VolumeOpacityOscillator : MonoBehaviour
{
    private Light2D light;

    public float min;
    public float max;
    public float increment;
    private float current;

    private bool comingFromMin = true;

    void Start()
    {
        light = GetComponent<Light2D>();

        current = min;
    }

    void Update()
    {
        if(current < max && comingFromMin) // need to increase volume opacity
        {
            current += Time.deltaTime * increment;
            if(current >= max)
            {
                comingFromMin = false;
            }
        }
        else // need to decrease volume opacity
        {
            current -= Time.deltaTime * increment;
            if(current <= min)
            {
                comingFromMin = true;
            }
        }

        light.intensity = current;
    }
}
