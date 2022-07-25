using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FadeInOnCreation : MonoBehaviour
{
    private UnityEngine.Rendering.Universal.Light2D light;
    private SpriteRenderer sprite;
    private float step = 0.25f;
    private float alpha = 0f;
    private float timer = 0f;

    void Start()
    {
        light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (timer < 10) // brightening stage
        {
            if (alpha < 1)
            {
                alpha += step * Time.deltaTime;
                sprite.color = new Color(1, 1, 1, alpha);
            }

            if (light.intensity < 1.5)
            {
                light.intensity += step * Time.deltaTime;
            }
        }
        else if(timer > 10) // darkening stage
        {
            if(alpha > 0)
            {
                alpha -= step * Time.deltaTime;
                sprite.color = new Color(1, 1, 1, alpha);
            }

            if (light.intensity > 0.5f)
            {
                light.intensity -= step * Time.deltaTime;
            }
        }
        else if(timer > 15)
        {
            Destroy(gameObject);
        }

        timer += Time.deltaTime;
        
    }
}
