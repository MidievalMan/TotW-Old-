using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{

    public float step;
    private float transition;
    private bool hasTouchedTrigger;
    public SpriteRenderer sprite;
    private Color color;

    public Color goalColor;

    private const float TRANSITION_VALUE_MIN = 0;
    private const float TRANSITION_VALUE_MAX = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            color = sprite.color;

            hasTouchedTrigger = true;
            transition = TRANSITION_VALUE_MIN;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sprite.color = Color.Lerp(color, goalColor, transition);
        }
    }

    void FixedUpdate()
    {
        if(hasTouchedTrigger && transition <= TRANSITION_VALUE_MAX)
        {
            transition += step;
        }
    }
}
