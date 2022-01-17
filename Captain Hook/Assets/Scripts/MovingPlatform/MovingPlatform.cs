using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public List<Transform> waypoints;
    public float moveSpeed;
    public int target;
    private bool pause = false;

    private void FixedUpdate()
    {
        if(!pause)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[target].position, moveSpeed * Time.deltaTime);
            if (transform.position == waypoints[target].position)
            {
                if (target == waypoints.Count - 1)
                {
                    target = 0;
                }
                else
                {
                    target++;
                }

                pause = true;
                StartCoroutine("Delay");

            }
        }

    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        pause = false;
    }
}
