using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RevealHidden : MonoBehaviour
{

    public Tilemap hidden;
    public BoundsInt area;
    
    public float alphaAmount;
    public float timeForChange;

    private Color c1;
    private Color c2;

    private void Start()
    {
        c1 = Color.white;
        c2 = new Color(255, 255, 255, alphaAmount);
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TileBase[] tileArray = hidden.GetTilesBlock(area);
        for(int i = 0; i < tileArray.Length; i++)
        {
            tileArray[i].SetColor
        }
        hidden.SetColor(Color.Lerp(c1, c2, timeForChange));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hidden.Color = Color.Lerp(c2, c1, timeForChange);
        hidden.SetColor(Color.Lerp(c2, c1, timeForChange));
    }*/
}
