using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatSky : MonoBehaviour
{
    [SerializeField]
    private Material material;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        BeatEffectManager.onBeatColorChanged += OnBeatColorChanged;
    }

    private void OnDisable()
    {
        BeatEffectManager.onBeatColorChanged -= OnBeatColorChanged;
    }

    private void OnBeatColorChanged(Color col)
    {
        if (!material)
            return;

        material.SetColor("_ColorB", col);
    }
}
