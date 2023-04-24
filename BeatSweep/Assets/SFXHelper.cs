using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXHelper : MonoBehaviour
{

    public AudioSource source;
    public AudioClip mineSFX;
    public AudioClip loseSFX;


    public void PlayMineSFX()
    {
        source.PlayOneShot(mineSFX);
    }

    public void PlayLoseSFX()
    {
        source.PlayOneShot(loseSFX);
    }

}
