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

    public void takeDamage( int amount )
    {
        playerHealth -= amount;
        playerHealth = Mathf.Clamp(playerHealth, 0, 100);

        if( playerHealth <= 0 )
        {
            playState = PlayState.isLosing;
        }

    }

}
