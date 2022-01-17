using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatforms : MonoBehaviour
{
    public PlatformEffector2D effector;
    // Start is called before the first frame update
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
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
        yield return new WaitForSeconds(0.1f);
        effector.rotationalOffset = 0f;
    }
}
