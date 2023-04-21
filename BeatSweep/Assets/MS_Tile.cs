using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MS_Tile : MonoBehaviour
{

    public Node node;    


    public GameObject StartZoneTileGameObject;
    public GameObject EndZoneTileGameObject;
    public GameObject ClosedTileGameObject;
    public GameObject OpenTileGameObject;

    public GameObject MineGameObject;

    public GameObject[] ConnectedMineCounterList;


    public void InitTileView()
    {

        Debug.Log("Start");

        StartZoneTileGameObject.SetActive(false);
        EndZoneTileGameObject.SetActive(false);
        ClosedTileGameObject.SetActive(false);
        OpenTileGameObject.SetActive(false);

        MineGameObject.SetActive(false);

        if ( node.isInStartZone )
        {
            StartZoneTileGameObject.SetActive(true);
        } else if(node.isInEndZone )
        {
            EndZoneTileGameObject.SetActive(true);
        } else {
            ClosedTileGameObject.SetActive(true);
        }

    }


    public void UpdateTileView()
    {

        Debug.Log("Update");
        foreach (var item in ConnectedMineCounterList)
        {
            item.SetActive(false);
        }

        if (node.hasMine)
        {
            MineGameObject.SetActive(true);
            return;
        } else
        {
            ConnectedMineCounterList[node.numConnectedMines].SetActive(true);
        }

        if( node.isHidden == false )
        {
            ClosedTileGameObject.SetActive(false);
            OpenTileGameObject.SetActive(true);
        } else
        {
            ClosedTileGameObject.SetActive(true);
            OpenTileGameObject.SetActive(false);
        }


        
    }


}
