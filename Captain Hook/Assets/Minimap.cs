using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Minimap : MonoBehaviour
{

    public Camera cam;
    public Button button;

    private float MAX_SIZE = 100f;
    private float MIN_SIZE = 1f;

    private bool holding = false;

    public ButtonType type;

    private float zoomStrength = 1.01f;

    public void OnPointerDown()
    {
        holding = true;
    }

    public void OnPointerUp()
    {
        holding = false;
    }

    void Update()
    {
        if(holding)
        {
            switch(type)
            {
                case ButtonType.ZoomIn:
                    if (cam.orthographicSize > MIN_SIZE)
                    {
                        cam.orthographicSize = cam.orthographicSize * (1f / zoomStrength);
                    }
                    break;
                case ButtonType.ZoomOut:
                    if (cam.orthographicSize < MAX_SIZE)
                    {
                        cam.orthographicSize = cam.orthographicSize * zoomStrength;
                    }
                    break;
            }

        }
    }
    
    public void DefaultZoom()
    {
        cam.orthographicSize = 14f;
    }

    public void HideMap()
    {
        cam.gameObject.SetActive(false);
        button.gameObject.SetActive(true);
    }

    public void ShowMap()
    {
        cam.gameObject.SetActive(true);
        button.gameObject.SetActive(false);
    }
    
}

    public enum ButtonType
    {
        ZoomIn,
        ZoomOut,
        Other,
    }
