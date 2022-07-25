using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    public Transform target;
    private Vector2 upperRightBound;
    private Vector2 lowerLeftBound;
    private GameObject currentRoom; // for locking camera movement

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(smoothedPosition.x, lowerLeftBound.x, upperRightBound.x),
            Mathf.Clamp(smoothedPosition.y, lowerLeftBound.y, upperRightBound.y),
            smoothedPosition.z
            );
        Debug.Log("upperRightBound" + upperRightBound);
        Debug.Log("lowerLeftBound" + lowerLeftBound);
        Debug.Log(smoothedPosition);
        transform.position = smoothedPosition;
    }

    public void SetUpperRightBound(Vector2 upperRight)
    {
        upperRightBound = upperRight;
    }

    public void SetLowerLeftBound(Vector2 lowerLeft)
    {
        lowerLeftBound = lowerLeft;
    }
}
