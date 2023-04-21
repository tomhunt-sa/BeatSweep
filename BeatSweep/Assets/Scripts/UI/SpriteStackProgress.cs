using UnityEngine;
using UnityEngine.UI;

public class SpriteStackProgress : MonoBehaviour
{
    public Graphic[] images;
    public bool Reverse;

    public void SetValue(float value)
    {
	    var spacing = 1.0f / images.Length;
        var compValue = Reverse ? 1.0f - value : value;
        
        var numActive = compValue / spacing;
        for (var i = 0; i < images.Length; i++)
        {
            if (!images[i])
                continue;
            
            images[i].enabled = i <= numActive;
        }
    }
}