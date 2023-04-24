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

    private void Update()
    {
        foreach (var strobe in strobes)
        {
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
