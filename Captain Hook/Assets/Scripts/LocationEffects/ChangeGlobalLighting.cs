using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangeGlobalLighting : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D globalLight;

    private float originalIntensity;
    public float goalIntensity;

    public float transitionStep;
    private float transitionValue;
    
    private bool hasTouchedBox = false;
    
    private const float TRANSITION_VALUE_MIN = 0;
    private const float TRANSITION_VALUE_MAX = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        originalIntensity = globalLight.intensity;

        if (collision.CompareTag("Player"))
        {
            globalLight.intensity = originalIntensity;
            transitionValue = TRANSITION_VALUE_MIN;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hasTouchedBox = true;
            globalLight.intensity = Mathf.Lerp(originalIntensity, goalIntensity, transitionValue);
        }
    }

    private void FixedUpdate()
    {
        if (hasTouchedBox && transitionValue < TRANSITION_VALUE_MAX)
        {
            if (transitionValue + transitionStep >= TRANSITION_VALUE_MAX)
            {
                transitionValue = TRANSITION_VALUE_MAX;
            }
            else
            {
                transitionValue += transitionStep;
            }
        }

        
    }
}
