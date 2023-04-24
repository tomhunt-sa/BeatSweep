using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayState
{
    notStarted,
    isPlaying,
    isLosing,
    isWinning,
    hasLost,
    hasWon
}


public class GameState : MonoBehaviour
{

    public int playerHealth;
    public PlayState playState = PlayState.notStarted;

    public static Action<bool> OnTakeDamage; 

    public void takeDamage( int amount, bool fromMine )
    {
        playerHealth -= amount;
        playerHealth = Mathf.Clamp(playerHealth, 0, 100);

        if( playerHealth <= 0 )
        {
            playState = PlayState.isLosing;
        }
        
        OnTakeDamage?.Invoke(fromMine);
    }


}
