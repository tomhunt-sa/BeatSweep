using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{

    public int playerHealth;

    public void takeDamage( int amount )
    {
        playerHealth -= amount;
    }

}
