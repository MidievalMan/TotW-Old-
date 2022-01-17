using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitate : MonoBehaviour
{
    private float counter;
    private float displacement;
    private float offSetRadians;

    public float magnitude = 0.01f;

    void Start()
    {
        offSetRadians = Random.Range(0f, 2f * Mathf.PI);
    }

    void FixedUpdate()
    {
        counter += 0.05f;
        displacement = magnitude * Mathf.Sin(counter + offSetRadians);
        transform.position += new Vector3(0, displacement, 0);
    }
}
