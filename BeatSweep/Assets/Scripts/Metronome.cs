using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour
{

    public int bpm;
    public AudioSource music;
    public float musicIntroTime;


    public float beatProgress = 0.0f;
    private float secondsPerBeat;

    private float lastBeatProgress;

    public int beatCount = 0;

    public bool isStartOfBeat = false;
    public bool isMetronoming = false;


    // Start is called before the first frame update
    void Awake()
    {
        secondsPerBeat = 60.0f / bpm;
    }

    public void StartMetronome()
    {
        music.Play();
        StartCoroutine(StartAfterWait());
    }

    private IEnumerator StartAfterWait()
    {
        yield return new WaitForSeconds(musicIntroTime);
        isMetronoming = true;        
    }


    public void StopMetronome()
    {
        music.Stop();
        isMetronoming = false;
    }

    // Update is called once per frame
    void Update()
    {

        if( isMetronoming )
        {
            beatProgress = (Time.time / secondsPerBeat) % 1.0f;
            if (beatProgress < lastBeatProgress)
            {
                beatCount++;
                isStartOfBeat = true;
            }
            else
            {
                isStartOfBeat = false;
            }

            lastBeatProgress = beatProgress;
        }
        
    }
}
