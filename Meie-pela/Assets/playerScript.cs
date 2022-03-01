using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class playerScript : MonoBehaviour
{
    [SerializeField] int energy = 2;

    public Grid grid;
    public Vector3Int previousPlayerGridLocation = new Vector3Int();

    private TileBase[] allTiles;
    private BoundsInt bounds;
    // Start is called before the first frame update
    void Start()
    {
        GameObject tileMap = GameObject.FindGameObjectWithTag("tilemap");
        allTiles = tileMap.GetComponent<tilemap_script>().allTiles;
        bounds = tileMap.GetComponent<tilemap_script>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int playerGridLocation = grid.WorldToCell(gameObject.transform.position);

        if (!playerGridLocation.Equals(previousPlayerGridLocation))
        {
            previousPlayerGridLocation = playerGridLocation;
        }
    }

    public void Move()
    {

    }
}
