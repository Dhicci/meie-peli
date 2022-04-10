using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class playerScript : MonoBehaviour
{
    public int energy = 3;
    [SerializeField] int playerHp = 3;

    public Grid grid;
    private GameObject tileMap;
    public Vector3Int previousPlayerGridLocation = new Vector3Int();
    public GameObject target;
    public GameObject fireBall;
    public bool casting = false;

    private TileBase[] allTiles;
    private BoundsInt bounds;
    private bool lerping = false;
    private Vector3 movementEnd;
    private Vector3 movementStart;
    private float moveTime;
    public bool myTurn;
    public TextMeshProUGUI playerEnergy;
    // Start is called before the first frame update
    void Start()
    {
        tileMap = GameObject.FindGameObjectWithTag("tilemap");
        /*allTiles = tileMap.GetComponent<tilemap_script>().allTiles;
        bounds = tileMap.GetComponent<tilemap_script>().bounds;*/
        UpdatePlayerPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (casting)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider != null)
                {
                    Debug.Log("casted");
                    //Debug.Log(hit.collider.gameObject.name);
                    if (hit.collider.gameObject.tag == "target")
                    {
                        Debug.Log("casted");
                        GameObject aFireBall = Instantiate(fireBall, gameObject.transform.position, Quaternion.identity);
                        aFireBall.GetComponent<fireballScript>().Launch(hit.collider.gameObject.transform.position, gameObject.transform.position);
                        GameObject[] destroyArray = GameObject.FindGameObjectsWithTag("target");
                        for (int i = destroyArray.Length - 1; i >= 0; i--)
                        {
                            Destroy(destroyArray[i]);
                        }
                        casting = false;
                        energy -= 1;
                    }
                }
            }
        }

        if (lerping)
        {
            moveTime += Time.deltaTime * 7;
            gameObject.transform.position = Vector2.Lerp(movementStart, new Vector3(movementEnd.x, movementEnd.y, 0), moveTime);
            if (gameObject.transform.position == new Vector3(movementEnd.x, movementEnd.y, 0))
            {
                lerping = false;
                moveTime = 0;
                UpdatePlayerPos();
            }
        }

        if(myTurn) {
            playerEnergy.text = energy.ToString();
        }

    }

    /*private bool EndOfTurn()
    {
        if(energy == 0)
        {
            myTurn = false;
            return true;
        } else
        {
            return false;
        }
        
    }*/

    public void UpdatePlayerPos()
    {
        Vector3Int playerGridLocation = grid.WorldToCell(gameObject.transform.position);
        
        if (!playerGridLocation.Equals(previousPlayerGridLocation))
        {
            previousPlayerGridLocation = playerGridLocation;
        }
    }


    //Move to a new position if that position is on an adjacent tile
    public void Move(Vector3Int mouseGridPos, string tileName)
    {
        if (casting)
        {
            return;
        }
        if (myTurn == false)
        {
            Debug.Log("my turn is false");
            return;
        }
        if (energy < 1)
        {
            return;
        }
        if (lerping == true)
        {
            return;
        }
        if (tileName == "Unpassable")
        {
            return;
        }
        if (previousPlayerGridLocation.x != mouseGridPos.x || previousPlayerGridLocation.y != mouseGridPos.y)
        {
            if (previousPlayerGridLocation.x == mouseGridPos.x + 1 || previousPlayerGridLocation.x == mouseGridPos.x - 1 || previousPlayerGridLocation.x == mouseGridPos.x)
            {
                if (previousPlayerGridLocation.y == mouseGridPos.y + 1 || previousPlayerGridLocation.y == mouseGridPos.y - 1 || previousPlayerGridLocation.y == mouseGridPos.y)
                {
                    if (previousPlayerGridLocation.y % 2 == 0)
                    {
                        if (false == (mouseGridPos.x - 1 == previousPlayerGridLocation.x && (mouseGridPos.y - 1 == previousPlayerGridLocation.y || mouseGridPos.y + 1 == previousPlayerGridLocation.y)))
                        {
                            movementEnd = grid.CellToWorld(mouseGridPos);
                            movementStart = gameObject.transform.position;
                            lerping = true;
                            energy--;
                        }
                    } else
                    {
                        if (false == (mouseGridPos.x + 1 == previousPlayerGridLocation.x && (mouseGridPos.y - 1 == previousPlayerGridLocation.y || mouseGridPos.y + 1 == previousPlayerGridLocation.y)))
                        {
                            movementEnd = grid.CellToWorld(mouseGridPos);
                            movementStart = gameObject.transform.position;
                            lerping = true;
                            energy--;
                        }
                    }
                    
                }
            }
        }
    }

    public bool MyTurn()
    {
        if (energy == 0)
        {
            UpdatePlayerPos();
            myTurn = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Cast()
    {
        if (casting)
        {
            return;
        }
        casting = true;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if ((x == 0 && y == 0) == false)
                {
                    if (previousPlayerGridLocation.y % 2 == 0)
                    {
                        if (false == ((previousPlayerGridLocation.x + x) - 1 == previousPlayerGridLocation.x && ((previousPlayerGridLocation.y + y) - 1 == previousPlayerGridLocation.y || (previousPlayerGridLocation.y + y) + 1 == previousPlayerGridLocation.y)))
                        {
                            Instantiate(target, grid.CellToWorld(new Vector3Int(previousPlayerGridLocation.x + x, previousPlayerGridLocation.y + y, previousPlayerGridLocation.z)), Quaternion.identity);
                        }
                    }
                    else
                    {
                        if (false == ((previousPlayerGridLocation.x + x) + 1 == previousPlayerGridLocation.x && ((previousPlayerGridLocation.y + y) - 1 == previousPlayerGridLocation.y || (previousPlayerGridLocation.y + y) + 1 == previousPlayerGridLocation.y)))
                        {
                            Instantiate(target, grid.CellToWorld(new Vector3Int(previousPlayerGridLocation.x + x, previousPlayerGridLocation.y + y, previousPlayerGridLocation.z)), Quaternion.identity);
                        }
                    }
                }
            }
        }
    }

    public void PlayerHealth(int deduction)
    {
        playerHp -= deduction;
        if (playerHp <= 0)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().GameOver(false);
        }
    }

}
