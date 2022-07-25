using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatforms : MonoBehaviour
{
    public PlatformEffector2D effector;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.S))
        {

            StartCoroutine("OneWay");
        }
    }

    IEnumerator OneWay()
    {
        effector.rotationalOffset = 180f;
        yield return new WaitForSeconds(0.25f);
        effector.rotationalOffset = 0f;
    }
}
