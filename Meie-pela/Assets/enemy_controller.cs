using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class enemy_controller : MonoBehaviour
{

    [SerializeField] int energy = 2;

    public Grid grid;
    public Tilemap tileMap;
    public GameObject target;
    public GameObject marker;

    private Vector3Int currentEnemyGridLocation;

    // Start is called before the first frame update
    void Start()
    {
        UpdateEnemyPos();
        FindPath();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    class pathTile
    {
        public pathTile previousTile;
        public Vector3Int thisGridLocation;
        public Vector3 thisLocation;
        public float targetDistance;
        public float startDistance;
        public float movementratio;
        public int tileDistance = 8000;
    }

    //Google A* pathfinding if you are interested why and how this code works
    //Dont touch anything in this code. It is fragile, scary, inefficent and will break easily
    private void FindPath()
    {
        int loopNumber = 1;
        Vector3 startLocation = grid.CellToWorld(currentEnemyGridLocation);
        Vector3 targetLocation = target.transform.position;
        Vector3Int currentGridLocation = currentEnemyGridLocation;
        Vector3 currentLocation = startLocation;
        
        List<pathTile> pathTiles = new List<pathTile>();
        List<pathTile> usedTiles = new List<pathTile>();
        //Vector3Int targetGridLocation = grid.WorldToCell(targetLocation);

        pathTile newTile = new pathTile();
        pathTile lastTile = newTile;
        
        bool normalSpot = false;
        while (currentGridLocation != grid.WorldToCell(targetLocation))
        {
            //Create a pathTile for every adjacent tile
            //This will help us to figure out the best path

            
            for (int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    normalSpot = false;
                    if ((x==0 && y==0)==false)
                    {
                        if (currentGridLocation.y % 2 == 0)
                        {
                            if (false == ((currentGridLocation.x + x) - 1 == currentGridLocation.x && ((currentGridLocation.y + y) - 1 == currentGridLocation.y || (currentGridLocation.y + y) + 1 == currentGridLocation.y)))
                            {
                                normalSpot = true;
                            }
                        } else
                        {
                            if (false == ((currentGridLocation.x + x) + 1 == currentGridLocation.x && ((currentGridLocation.y + y) - 1 == currentGridLocation.y || (currentGridLocation.y + y) + 1 == currentGridLocation.y)))
                            {
                                normalSpot = true;
                            }
                        }
                        if (normalSpot == true)
                        {
                            TileBase aTile = tileMap.GetTile(new Vector3Int(currentGridLocation.x + x, currentGridLocation.y + y, 0));
                            string tileNameI;
                            if (aTile == null)
                            {
                                tileNameI = "Unpassable";
                            }
                            else
                            {
                                tileNameI = aTile.name;
                            }
                            if (loopNumber < 3)
                            {
                                if (tileNameI != "Unpassable")
                                {
                                    newTile = new pathTile();
                                    newTile.previousTile = lastTile;

                                    newTile.thisGridLocation = new Vector3Int(currentGridLocation.x + x, currentGridLocation.y + y, 0);
                                    newTile.thisLocation = grid.CellToWorld(newTile.thisGridLocation);
                                    newTile.targetDistance = Vector3.Distance(newTile.thisLocation, targetLocation);
                                    newTile.startDistance = Vector3.Distance(newTile.thisLocation, startLocation);
                                    newTile.movementratio = newTile.targetDistance + newTile.startDistance;
                                    newTile.tileDistance = loopNumber;
                                    pathTiles.Add(newTile);
                                }

                            }
                            else
                            {
                                if (lastTile.previousTile.thisGridLocation != new Vector3Int(currentGridLocation.x + x, currentGridLocation.y + y, 0))
                                {
                                    if (tileNameI != "Unpassable")
                                    {
                                        newTile = new pathTile();
                                        newTile.previousTile = lastTile;

                                        newTile.thisGridLocation = new Vector3Int(currentGridLocation.x + x, currentGridLocation.y + y, 0);
                                        newTile.thisLocation = grid.CellToWorld(newTile.thisGridLocation);
                                        newTile.targetDistance = Vector3.Distance(newTile.thisLocation, targetLocation);
                                        newTile.startDistance = Vector3.Distance(newTile.thisLocation, startLocation);
                                        newTile.movementratio = newTile.targetDistance + newTile.startDistance;
                                        newTile.tileDistance = loopNumber;
                                        pathTiles.Add(newTile);
                                    }

                                }
                            }
                        }
                    }
                }
            }

            int middleman = 0;
            float myRatio = 1000;
            
            for (int i = 0; i < pathTiles.Count; i++)
            {
                if (pathTiles[i].tileDistance == loopNumber)
                {
                    if (pathTiles[i].targetDistance < pathTiles[middleman].targetDistance/*)*/)
                    {
                        bool allowAddition = true;
                        foreach (pathTile JTile in usedTiles)
                        {
                            if (pathTiles[i].thisGridLocation == JTile.thisGridLocation)
                            {
                                allowAddition = false;
                            }
                        }
                        if (allowAddition == true)
                        {
                            middleman = i;
                            myRatio = pathTiles[i].movementratio;
                        }
                    }

                }
            }
            lastTile = pathTiles[middleman];
            usedTiles.Add(pathTiles[middleman]);
            //Instantiate(marker, pathTiles[middleman].thisLocation, Quaternion.identity);
            currentGridLocation = pathTiles[middleman].thisGridLocation;
            currentLocation = pathTiles[middleman].thisLocation;
            loopNumber++;
            if (loopNumber == 30)
            {
                return;
            }
        }
        bool there = false;
        pathTile lastpathTile = usedTiles[usedTiles.Count - 1];
        
        while(there == false)
        {
            if (lastpathTile.previousTile.tileDistance == 8000)
            {
                there = true;
            } else
            {
                Instantiate(marker, lastpathTile.previousTile.thisLocation, Quaternion.identity);
                lastpathTile = lastpathTile.previousTile;
            }
        }
    }

    public void UpdateEnemyPos()
    {
        Vector3Int enemyGridLocation = grid.WorldToCell(gameObject.transform.position);

        if (!enemyGridLocation.Equals(currentEnemyGridLocation))
        {
            currentEnemyGridLocation = enemyGridLocation;
        }
    }
}
