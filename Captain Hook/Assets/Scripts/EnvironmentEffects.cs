using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentEffects : MonoBehaviour
{

    public Camera cam;
    public GameObject forestLight;

    private float x;
    private float y;
    private Vector3 pos;

    public Area area;

    private float timer = 0f;

    void Update()
    {
        if(area == Area.Forest)
        {
            if (timer > 0.5)
            {
                x = Random.Range(-1f, 2f); // 0 is left side, 1 is right side
                y = Random.Range(-0.5f, 1.5f); // 0 is bottom, 1 is top
                pos = new Vector3(x, y, 1);
                pos = Camera.main.ViewportToWorldPoint(pos);

                GameObject.Instantiate(forestLight, pos, Quaternion.identity);

                timer = 0f;
            }

            timer += Time.deltaTime;
        }
    }
}

public enum Area
{
    Cavern,
    Town,
    Grassland,
    Forest
}
