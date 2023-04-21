using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempoBar : MonoBehaviour
{

    public Image bar;
    private RectTransform transform;

    //public Metronome metronome;

    private float startPosX;
    private Vector2 oldSizeDelta;
    
    void Awake()
    {
        transform = bar.rectTransform;

        oldSizeDelta = transform.sizeDelta;
    }

    public void SetScale( float scale )
    {
        //float newWidth = startWidth * scale;        
        transform.sizeDelta = new Vector2(oldSizeDelta.x * scale, oldSizeDelta.y);

    }

}
