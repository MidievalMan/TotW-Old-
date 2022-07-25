using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class DisableCellColliders : MonoBehaviour
{
    private Tilemap tilemap;

    void Start()
    {
        Tilemap tilemap = GetComponent<Tilemap>();

        tilemap.CompressBounds();

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    //tilemap
                    Debug.Log("T");
                } else
                {
                    Debug.Log("X");
                }
            }
        }
    }
}
