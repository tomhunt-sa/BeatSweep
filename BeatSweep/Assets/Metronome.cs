using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour
{

    public int bpm;


    public float beatProgress = 0.0f;
    private float secondsPerBeat;

    // Start is called before the first frame update
    void Awake()
    {
        secondsPerBeat = 60.0f / bpm;
    }

    // Update is called once per frame
    void Update()
    {
        beatProgress = (Time.time * secondsPerBeat * 4) % 1;        
    }
}
