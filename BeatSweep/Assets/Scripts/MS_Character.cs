using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum CharacterState
{
    Walk,
    FallOver
}

public class MS_Character : MonoBehaviour
{
    [SerializeField] private SpriteAnimator spriteAnimator;
    
    //public Grid grid;

    public Node[] path;

    public float speed;

    public CharacterState State; 
    
    public SpriteAnimationClip walkClip;
    public SpriteAnimationClip fallOverClip;
    
    private GameState gameState;

    [SerializeField]
    private float damagedDuration = 1;
    private float damagedTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        gameState = FindObjectOfType<GameState>();

        GameState.OnTakeDamage += OnTakeDamage;
    }

    private void OnDestroy()
    {
        GameState.OnTakeDamage -= OnTakeDamage;
    }

    private void OnTakeDamage(bool fromMine)
    {
        if (fromMine)
        {
            damagedTimer = damagedDuration;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameState)
            return;
        
        if (!spriteAnimator)
            return;

        if (damagedTimer >= 0)
        {
            damagedTimer -= Time.deltaTime;
            SetState(CharacterState.FallOver);
            return;
        }

        SetState(gameState.playerHealth > 0 ? CharacterState.Walk : CharacterState.FallOver);
    }

    private void SetState(CharacterState state)
    {
        if (this.State == state)
            return;

        State = state;
        
        switch (state)
        {
            case CharacterState.Walk:
                spriteAnimator.Clip = walkClip;
                break;
            case CharacterState.FallOver:
                spriteAnimator.Clip = fallOverClip;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    public void MoveToNode( Node node )
    {
        transform.position = new Vector3(  );
    }

    public void WalkAlongPath( Node[] path )
    {
        DOTween.Kill(gameObject);

        Vector3[] waypoints = new Vector3[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            waypoints[i] = new Vector3(path[i].gridX, 0, path[i].gridY);
        }

        transform.DOPath(waypoints, speed * waypoints.Length, PathType.Linear);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }


    public void SetPosition( Vector3 target )
    {
        transform.position = target;
    }


}
