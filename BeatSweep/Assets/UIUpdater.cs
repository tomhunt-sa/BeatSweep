using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIUpdater : MonoBehaviour
{

    public GameState gameState;

    public TMP_Text healthText;

  
    // Update is called once per frame
    void Update()
    {
        healthText.text = gameState.playerHealth.ToString();        
    }
}
