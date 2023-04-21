using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRunner : MonoBehaviour
{

    public Grid grid;
    public int startZoneSize;
    public int endZoneSize;
    public int startingHealth;
    public int beatMissDamage;
    public int mineHitDamage;

    public float chanceOfMine;

    
    public MS_Tile tileGameobject;
    public MS_Character characterGameobject;

    public GameState gameState;
    public Metronome metronome;


    public List<MS_Tile> tilesList;


    // Start is called before the first frame update
    void Start()
    {

        tilesList = new List<MS_Tile>();

        grid.CreateGrid();
        PopulateGrid();

        UpdateGridView();

        gameState.playerHealth = startingHealth;


    }


    void PopulateGrid()
    {
        for( int x=0; x<grid.gridSizeX; x++ )
        {
            for (int y = 0; y < grid.gridSizeY; y++)
            {
                //Debug.Log( string.Format("{0}, {1}", x, y) );

                GameObject tile = Instantiate(tileGameobject.gameObject);
                tile.transform.position = new Vector3(x, 0, y);

                MS_Tile msTile = tile.GetComponent<MS_Tile>();
                Node node = grid.grid[x, y];
                msTile.node = node;

                if( y < startZoneSize )
                {
                    node.isInStartZone = true;
                }

                if (y > grid.gridSizeY - endZoneSize)
                {
                    node.isInEndZone = true;
                }

                float r = Random.value;
                if( r <= chanceOfMine )
                {
                    if( !node.isInEndZone && !node.isInStartZone)
                    {
                        node.hasMine = true;
                    }                    
                }

                tilesList.Add(msTile);
                msTile.InitTileView();

            }
        }

        grid.UpdateMineNumbers();

    }


    


    void UpdateGridView()
    {        
        foreach (var tile in tilesList)
        {
            tile.UpdateTileView();
        }
    }
    




    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    MS_Tile tile = hit.collider.gameObject.GetComponent<MS_Tile>();
                    Node node = tile.node;

                    if( node.hasMine )
                    {
                        node.mineIsVisible = true;
                        HitMine(node);
                    } else
                    {
                        grid.UpdateHiddenStateAtNode(node);                        
                    }

                    UpdateGridView();
                }
            }
        }
    }

    void HitMine(Node node)
    {
        gameState.takeDamage(mineHitDamage);
        Debug.Log(string.Format("Hit Mine! Health is now {0} after taking {1} damage!", gameState.playerHealth, mineHitDamage));
    }
}
