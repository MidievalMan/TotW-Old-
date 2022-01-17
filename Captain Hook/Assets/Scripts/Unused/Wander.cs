using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{

    public float speed;
    public float timeBetween;

    public float speedVarianceMin;
    public float speedVarianceMax;
    private float speedVariance;

    private float timer;
    private int rand;

    private Rigidbody2D rb;
    private Vector2 force;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (timer >= timeBetween)
        {
            speedVariance = Random.Range(speedVarianceMin, speedVarianceMax);

            rand = Random.Range(0, 2);
            if (rand == 0)
            {
                force = Vector2.right;
            }
            else
            {
                force = Vector2.left;
            }
            rb.AddForce(force * speed * speedVariance);

            timer = 0f;
        }
    }
    private void FixedUpdate()
    {
        timer += 0.02f;
    }
}
