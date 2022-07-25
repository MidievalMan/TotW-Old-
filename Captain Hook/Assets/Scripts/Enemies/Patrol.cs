using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed;

    private bool movingRight;

    private float distance = 1f;

    public Transform groundDetection;

    private Vector2 moveDir = Vector2.right;

    private void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);

        if (groundInfo.collider == false)
        {
            if (movingRight)
            {
                moveDir = Vector2.left;
                movingRight = false;
            }
            else
            {
                moveDir = Vector2.right;
                movingRight = true;
            }
        }
    }
}
