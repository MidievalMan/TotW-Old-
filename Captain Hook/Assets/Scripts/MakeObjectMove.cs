using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeObjectMove : MonoBehaviour
{

    public GameObject go;
    public string tagForCompare;
    public Vector2 move;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(go.CompareTag(tagForCompare))
        {
            var rb = go.GetComponent<Rigidbody2D>();
            rb.position += move;
        }
    }

}
