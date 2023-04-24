using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMover : MonoBehaviour
{

    public int visibleTiles;
    public int bottomBuffer;

    public float speed;

    private int lastPosition;
    private int moveCount;

    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
    }

    public void MoveToNextScreen(Vector3 targetPosition)
    {

        int cutoff = lastPosition + visibleTiles;
        int target = (int)targetPosition.z;


        //Debug.Log(string.Format("{0}, {1}", target, cutoff));

        if( target > cutoff )
        {
            //Vector3 newPosition = startPosition + new Vector3(0, 0, (float)(target-bottomBuffer));
            transform.DOMoveZ(startPosition.z + (float)(target - bottomBuffer), speed );
            lastPosition = target - bottomBuffer - (bottomBuffer * (moveCount-1));
            moveCount++;
        }

        

    }

  


}
