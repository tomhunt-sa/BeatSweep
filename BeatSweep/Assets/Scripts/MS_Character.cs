using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MS_Character : MonoBehaviour
{

    //public Grid grid;

    public Node[] path;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToNode( Node node )
    {
        transform.position = new Vector3(  );
    }

    public void WalkAlongPath( Node[] path )
    {
        Vector3[] waypoints = new Vector3[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            waypoints[i] = new Vector3(path[i].gridX, 0, path[i].gridY);
        }

        transform.DOPath(waypoints, speed * waypoints.Length, PathType.Linear);
    }


}
