using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempoBar : MonoBehaviour
{

    public Image bar;
    private RectTransform transform;

    public Metronome metronome;

    private float startPosX;
    private float startWidth;
    
    void Awake()
    {
        transform = bar.rectTransform;

        startPosX = transform.rect.position.x;
        startWidth = transform.rect.width;
    }

    private void Update()
    {
        float scale = metronome.beatProgress;
        ScaleFromLeft(scale);

    }

    private void ScaleFromLeft( float scale )
    {
        float newWidth = startWidth * scale;
        Vector2 oldSizeDelta = transform.sizeDelta;
        transform.sizeDelta = new Vector2(newWidth, oldSizeDelta.y);
    }

}
