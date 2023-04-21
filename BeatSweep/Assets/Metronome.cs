using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour
{

    public int bpm;


    private float beatProgress = 0.0;
    private float msPerBeat;

    // Start is called before the first frame update
    void Awake()
    {
        msPerBeat = 60000 / bpm;
    }

    // Update is called once per frame
    void Update()
    {
        int ms = 
    }
}
