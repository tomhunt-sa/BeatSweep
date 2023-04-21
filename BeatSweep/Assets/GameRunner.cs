using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRunner : MonoBehaviour
{

    public Grid grid;
    public int startZoneSize;
    public int endZoneSize;

    public float chanceOfMine;

    public MS_Tile TileGameobject;
    public MS_Character CharacterGameobject;

    public List<MS_Tile> tilesList;


    // Start is called before the first frame update
    void Start()
    {

        tilesList = new List<MS_Tile>();

        grid.CreateGrid();
        PopulateGrid();

        UpdateGridView();



    }


    void PopulateGrid()
    {
        for( int x=0; x<grid.gridSizeX; x++ )
        {
            for (int y = 0; y < grid.gridSizeY; y++)
            {
                //Debug.Log( string.Format("{0}, {1}", x, y) );

                GameObject tile = Instantiate(TileGameobject.gameObject);
                tile.transform.position = new Vector3(x, 0, y);

                MS_Tile msTile = tile.GetComponent<MS_Tile>();
                msTile.node = grid.grid[x, y];

                if( y < startZoneSize )
                {
                    msTile.node.isInStartZone = true;
                }

                if (y > grid.gridSizeY - endZoneSize)
                {
                    msTile.node.isInEndZone = true;
                }

                float r = Random.value;
                if( r <= chanceOfMine )
                {
                    msTile.node.hasMine = true;
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
                    grid.UpdateHiddenStateAtNode(node);
                    UpdateGridView();
                }
            }
        }
    }
}
