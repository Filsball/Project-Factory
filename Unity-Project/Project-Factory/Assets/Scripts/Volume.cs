using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{

    public Slider sliderInstance;

    public void Start()
    {
        if (sliderInstance == null)
        {
            return;
        }
        sliderInstance.value = AudioListener.volume;
    }

    public void ChangeVol(float newValue)
    {
        float newVol = AudioListener.volume;
        newVol = newValue;
        AudioListener.volume = newVol;
    }

    
}
