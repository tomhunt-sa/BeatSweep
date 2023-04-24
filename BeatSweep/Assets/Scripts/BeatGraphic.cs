using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class BeatGraphic : MonoBehaviour
{
    private Graphic graphic;
    private float alpha;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        BeatEffectManager.onBeatColorChanged += OnBeatColorChanged;

        graphic = GetComponent<Graphic>();
        alpha = graphic.color.a;
    }

    private void OnDisable()
    {
        BeatEffectManager.onBeatColorChanged -= OnBeatColorChanged;
    }

    private void OnBeatColorChanged(Color col)
    {
        if (!graphic)
            return;

        col.a = alpha;
        graphic.color = col;
    }
}
