using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{

    public int gridSizeX, gridSizeY;
    



    public Node[,] grid;
    public List<Node> path;

    private float nodeDiameter;

    private int searchArea = 3;
    private int offset;


    private void Start()
    {
        offset = (searchArea - 1) / 2;
    }

    public void CreateGrid()
    {

        Debug.Log("Creating grid...");

        grid = new Node[gridSizeX, gridSizeY];
        //Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {                                
                grid[x, y] = new Node(true, x, y);
            }
        }
    }

    public Node[] flattenGrid()
    {
        Node[] flattenedArray = new Node[gridSizeX * gridSizeY];



        return flattenedArray;

    }

    public void UpdateMineNumbers()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                UpdateNumberOfConnectedMinesAtNode(grid[x, y]);
            }
        }
    }


    public void UpdateNumberOfConnectedMinesAtNode( Node node )
    {

        if (node.hasMine) return;

        int numMines = 0;        

        for (int x = node.gridX - offset; x < (node.gridX - offset) + searchArea; x++)
        {
            for (int y = node.gridY - offset; y < (node.gridY - offset) + searchArea; y++)
            {
                if( x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY )
                {
                    Node nodeForInspection = grid[x, y];
                    if (nodeForInspection.hasMine)
                    {
                        numMines++;
                    }
                }
                               
            }
        }
        
        node.numConnectedMines = numMines;
        
    }

    public void UpdateHiddenStateAtNode(Node node)
    {
        
        node.isHidden = false;
        //node.isInStartZone = false;
        

        if (node.numConnectedMines == 0)
        {
            for (int x = node.gridX - offset; x < (node.gridX - offset) + searchArea; x++)
            {
                for (int y = node.gridY - offset; y < (node.gridY - offset) + searchArea; y++)
                {
                    if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
                    {
                        Node nodeForInspection = grid[x, y];
                        if (nodeForInspection.numConnectedMines == 0)
                        {
                            if (nodeForInspection.isHidden)
                            {
                                UpdateHiddenStateAtNode(nodeForInspection);                                
                            }

                        }
                    }

                }
            }
        } else
        {
            node.isOnBoundary = true;
        }
    }


    public Node GetFurthestReachableNode()
    {

        for (int y = gridSizeY-1; y >= 0; y--)
        {
            for (int x = gridSizeX-1; x >= 0; x--)
            {
                Node nodeForInspection = grid[x, y];
                if (!nodeForInspection.isHidden)
                {

             
                    Debug.Log(string.Format("{0} {1}", x, y));
                    //nodeForInspection.isPathfinderTarget = true;
                    return nodeForInspection;
                }
            }
        }
        Debug.Log(string.Format("Could not find a tile"));
        return null;
    }


    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

      

        // only check a cross here!


        int checkXPlus = node.gridX++;
        int checkXMinus = node.gridX--;
        int checkYPlus = node.gridY++;
        int checkYMinus = node.gridY--;

        int[] checks = new int[] { checkXPlus, checkXMinus, checkYPlus, checkYMinus };

        //foreach (var check in checks)
        //{
        //    if (check >= 0 && check < gridSizeX && checkY >= 0 && checkY < gridSizeY)
        //    {
        //        neighbors.Add(grid[checkX, checkY]);
        //    }
        //}





        return neighbors;
    }

    //public Node NodeFromWorldPoint(Vector3 worldPosition)
    //{
    //    float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
    //    float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
    //    percentX = Mathf.Clamp01(percentX);
    //    percentY = Mathf.Clamp01(percentY);

    //    int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
    //    int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
    //    return grid[x, y];
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

    //    if (grid != null)
    //    {
    //        foreach (Node n in grid)
    //        {
    //            Gizmos.color = (n.walkable) ? Color.white : Color.red;
    //            if (path != null)
    //            {
    //                if (path.Contains(n))
    //                {
    //                    Gizmos.color = Color.black;
    //                }
    //            }
    //            Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
    //        }
    //    }
    //}
}