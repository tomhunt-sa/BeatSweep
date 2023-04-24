using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRunner : MonoBehaviour
{

    public SplashDialog splashDialog;
    public WinDialog winDialog;
    public LoseDialog loseDialog;


    public Grid grid;
    public Astar pathfinder;
    public int startZoneSize;
    public int endZoneSize;
    public int startingHealth;
    public int beatMissDamage;
    public int beatHitDamage;
    public int mineHitDamage;
    public float beatTolerance;

    public float chanceOfMine;

    
    public MS_Tile tileGameobject;
    public MS_Character character;

    public GameState gameState;
    public Metronome metronome;

    public CameraMover cameraMover;


    public List<MS_Tile> tilesList;

    public int lastBeatHit;
    public int hitBeatCount;

    public GameObject tilesContainer;

    // Start is called before the first frame update
    void Start()
    {

        splashDialog.gameObject.SetActive(true);
        winDialog.gameObject.SetActive(false);
        loseDialog.gameObject.SetActive(false);

    }

    public void ResetGame()
    {
        Debug.Log("RESET");
        if (tilesContainer != null)
        {
            foreach (Transform t in tilesContainer.transform)
            {
                GameObject.Destroy(t.gameObject);
            }
        }

        gameState.playState = PlayState.notStarted;
    }


    public void StartGame()
    {

        Debug.Log("STARTING GAME");
        splashDialog.gameObject.SetActive(false);
        winDialog.gameObject.SetActive(false);
        loseDialog.gameObject.SetActive(false);

        tilesList = new List<MS_Tile>();

        grid.CreateGrid();
        PopulateGrid( tilesContainer );

        UpdateGridView();

        gameState.playerHealth = startingHealth;

        float center = Mathf.Round(grid.gridSizeX / 2.0f) - 1;
        character.SetPosition(new Vector3(center, 0, 0));
        cameraMover.SetPositionX(center);

        gameState.playState = PlayState.isPlaying;
    }

    public void WinGame()
    {
        Debug.Log("WIN!!");
        ResetGame();
    }

    public void LoseGame()
    {
        Debug.Log("LOSE!!");
        ResetGame();
        Node currentNode = grid.NodeFromWorldPoint(character.GetPosition());
        int nodeY = currentNode.gridY;
        int percent = (int)(((float)nodeY / (float)grid.gridSizeY) * 100);

        //Debug.Log(string.Format("{0} {1} {2}", nodeY, grid.gridSizeY, percent ));

        loseDialog.gameObject.SetActive(true);
        loseDialog.SetPercentageComplete(percent);
    }



    void PopulateGrid( GameObject container )
    {
        for( int x=0; x<grid.gridSizeX; x++ )
        {
            for (int y = 0; y < grid.gridSizeY; y++)
            {
                //Debug.Log( string.Format("{0}, {1}", x, y) );

                GameObject tile = Instantiate(tileGameobject.gameObject);
                tile.transform.position = new Vector3(x, 0, y);
                tile.name = string.Format("x:{0} y:{1}", x, y);
                tile.transform.SetParent(container.transform);

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

        if( gameState.playState == PlayState.hasLost )
        {
            LoseGame();
            return;
        }
        else
        if (gameState.playState == PlayState.hasWon)
        {
            WinGame();
            return;
        }
        else
        if(gameState.playState == PlayState.notStarted)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {

                    ResetTiles();

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

                    bool hitBeat = metronome.beatProgress > 1.0 - beatTolerance;
                    if( !hitBeat )
                    {
                        MissBeat();
                    } else
                    {
                        HitBeat();
                    }

                    lastBeatHit = metronome.beatCount + 1;


                    Node furthestNode = grid.GetFurthestReachableNode();
                    furthestNode.isPathfinderTarget = true;

                    Node[] path = pathfinder.FindPath(grid.NodeFromWorldPoint( character.GetPosition() ), furthestNode);
                    
                    if(path != null)
                    {
                        foreach (var pathnode in path)
                        {
                            pathnode.isPathfinderTarget = true;
                        }
                    }

                    if( path != null )
                    {
                        character.WalkAlongPath(path);
                    }
                    

                    UpdateGridView();
                    
                }
            }
        }

        if( metronome.isStartOfBeat )
        {
            int beatDiff = Mathf.Abs(metronome.beatCount - lastBeatHit);
            if ( beatDiff > 0 )
            {
                MissBeat();
                //Debug.Log(string.Format("Last beat: {0} | Current beat: {1} | beatDiff: {2}", lastBeatHit, metronome.beatCount, beatDiff));
                lastBeatHit = metronome.beatCount;
            }
        }


        cameraMover.MoveToNextScreen(character.GetPosition());

    }

    void HitMine(Node node)
    {
        gameState.takeDamage(mineHitDamage);
        hitBeatCount = 0;
        //Debug.Log(string.Format("Hit Mine! Health is now {0} after taking {1} damage!", gameState.playerHealth, mineHitDamage));
    }

    void MissBeat()
    {
        gameState.takeDamage(beatMissDamage);
        //Debug.Log(string.Format("Missed a beat! Health is now {0} after taking {1} damage!", gameState.playerHealth, beatMissDamage));
        hitBeatCount = 0;       
    }

    void HitBeat()
    {
        hitBeatCount++;
        if( hitBeatCount > 1 )
        {
            gameState.takeDamage(beatHitDamage );
        }
            
    }

    private void ResetTiles()
    {
        foreach (var tile in tilesList)
        {
            Node node = tile.node;
            node.isPathfinderTarget = false;
        }
    }
}
