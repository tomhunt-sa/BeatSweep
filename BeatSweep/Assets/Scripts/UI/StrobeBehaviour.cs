using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Strobe
{
    public float StrobeHz = 50;
    public float Timer;
    public bool IsOn = true;
    public bool ToggleWithBeat;
}

[RequireComponent(typeof(CanvasGroup))]
public class StrobeBehaviour : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private List<Strobe> strobes = new ();
    
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        BeatEffectManager.onBeat += OnBeat;
    }
    
    private void OnDisable()
    {
        BeatEffectManager.onBeat -= OnBeat;
    }

    private void OnBeat()
    {
        foreach (var strobe in strobes)
        {
            if (!strobe.ToggleWithBeat)
                continue;

            strobe.IsOn = !strobe.IsOn;
        }
    }

    
    private void Update()
    {
        foreach (var strobe in strobes)
        {
            if (strobe.ToggleWithBeat)
                continue;

            strobe.Timer += Time.deltaTime;

            var strobeDelta = 1.0f / strobe.StrobeHz;

            while (strobe.Timer > strobeDelta)
            {
                strobe.Timer -= strobeDelta;
                strobe.IsOn = !strobe.IsOn;
            }
        }
        
        if (canvasGroup)
            canvasGroup.alpha = strobes.All(x => x.IsOn) ? 1 : 0;
    }
}
