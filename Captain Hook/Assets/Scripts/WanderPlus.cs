using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderPlus : MonoBehaviour
{

    private Vector3 pos;
    private Vector3 currentPos;
    private Vector3 target;

    private float xDiff;
    private float yDiff;

    private float speed = 1f;

    void Start()
    {
        pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        FindTarget();
    }

    void Update()
    {
        pos = Vector3.MoveTowards(pos, target, speed * Time.deltaTime);
        transform.Translate(pos - transform.position);

        if(Vector2.Distance(pos, target) < 0.01f)
        {
            FindTarget();
        }
    }

    private void FindTarget()
    {
        xDiff = Random.Range(-5f, 5f);
        yDiff = Random.Range(-5f, 5f);
        target = new Vector3(pos.x + xDiff, pos.y + yDiff, 1);
    }
}
