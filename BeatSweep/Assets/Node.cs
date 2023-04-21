using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;
    public int gCost;
    public int hCost;
    public Node parent;

    public bool hasMine;
    public bool isInStartZone = false;
    public bool isInEndZone = false;
    public int numConnectedMines = 0;
    public bool isHidden = true;

    public Node(bool walkable, int gridX, int gridY)
    {
        this.walkable = walkable;
        //this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    


}