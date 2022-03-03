using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class tilemap_script : MonoBehaviour
{
    private playerScript playerScript;
    public TileBase[] allTiles;
    public BoundsInt bounds;
    public Grid grid;

    public GameObject cursorSprite;
    public Vector3 previousMousePos = new Vector3();
    void Awake()
    {

        Tilemap tilemap = GetComponent<Tilemap>();
        tilemap.CompressBounds();

        bounds = tilemap.cellBounds;
        allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    //Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                }
                /*else
                {
                    Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                }*/
            }
        }

        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<playerScript>();
    }

    public void Update()
    {
        
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinateFalse = grid.WorldToCell(mouseWorldPos);
        
        Vector3 coordinate = grid.CellToWorld(coordinateFalse);

        if (!coordinate.Equals(previousMousePos))
        {
            //Debug.Log(coordinateFalse);
            Destroy(GameObject.Find("CursorObject(Clone)"));
            Instantiate(cursorSprite, new Vector3(coordinate.x, coordinate.y, -1), Quaternion.identity);
            previousMousePos = coordinate;
        }

        if (Input.GetMouseButtonDown(0))
        {
            playerScript.Move(coordinateFalse);
        }
    }

}
