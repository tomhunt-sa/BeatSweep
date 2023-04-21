using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class UIUpdater : MonoBehaviour
{

    public GameRunner gameRunner;
    public GameState gameState;

    public TMP_Text healthText;
    public TempoBar tempoBar;
    
    public SpriteStackProgress TempoSpriteStackProgress;

    public RectTransform tempoBarSafeArea;

    public RectTransform beatPulseObject;
    public float pulsePunchTrigger = 0.9f;
    public Vector3 pulsePunchScale = new Vector3(0.1f, 0.1f, 0.1f);
    public float pulsePunchDuration = 0.2f;



    public TMP_Text lastBeatHit;
    public TMP_Text currentBeat;


    private void Awake()
    {
        Vector2 oldDelta = tempoBarSafeArea.sizeDelta;
        tempoBarSafeArea.sizeDelta = new Vector2(oldDelta.x * gameRunner.beatTolerance, oldDelta.y) ;
    }

    private float prevBeatProgress;
    
    // Update is called once per frame
    void Update()
    {
        
        healthText.text = gameState.playerHealth.ToString();
        tempoBar.SetScale( gameRunner.metronome.beatProgress );

        if (prevBeatProgress < pulsePunchTrigger && gameRunner.metronome.beatProgress >= pulsePunchTrigger)
        {
            if (beatPulseObject)
            {
                beatPulseObject.DOPunchScale(pulsePunchScale, pulsePunchDuration);
            }
        }
        
        if (TempoSpriteStackProgress)
            TempoSpriteStackProgress.SetValue(gameRunner.metronome.beatProgress);


        lastBeatHit.text = gameRunner.lastBeatHit.ToString();
        currentBeat.text = gameRunner.metronome.beatCount.ToString();

        prevBeatProgress = gameRunner.metronome.beatProgress;
    }
}
