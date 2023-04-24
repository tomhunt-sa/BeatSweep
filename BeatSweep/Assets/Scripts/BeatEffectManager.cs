using System;
using DG.Tweening;
using UnityEngine;

public class BeatEffectManager : MonoBehaviour
{
    public Color beatColor;

    public float hueStep = 0.1f;

    static public Action<Color> onBeatColorChanged;
    static public Action onBeat;
    
    public void HandleBeat()
    {
        Color.RGBToHSV(beatColor, out var h, out var s, out var v);

        h += hueStep;
        while (h > 1)
        {
            h -= 1;
        }

        var oldBeatColor = beatColor; 
        beatColor = Color.HSVToRGB(h, s, v);

        DOVirtual.Color(oldBeatColor, beatColor, 0.2f, x => onBeatColorChanged(x));
        
        onBeat?.Invoke();
    }
}
