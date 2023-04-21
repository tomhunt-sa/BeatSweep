using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIUpdater : MonoBehaviour
{

    public GameRunner gameRunner;
    public GameState gameState;

    public TMP_Text healthText;
    public TempoBar tempoBar;
    public RectTransform tempoBarSafeArea;


    public TMP_Text lastBeatHit;
    public TMP_Text currentBeat;


    private void Awake()
    {
        Vector2 oldDelta = tempoBarSafeArea.sizeDelta;
        tempoBarSafeArea.sizeDelta = new Vector2(oldDelta.x * gameRunner.beatTolerance, oldDelta.y) ;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = gameState.playerHealth.ToString();
        tempoBar.SetScale( gameRunner.metronome.beatProgress );


        lastBeatHit.text = gameRunner.lastBeatHit.ToString();
        currentBeat.text = gameRunner.metronome.beatCount.ToString();
    }
}