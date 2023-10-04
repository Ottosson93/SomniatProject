using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lucidity : MonoBehaviour
{

    private Slider slider;

    public void Start()
    {
        slider = FindAnyObjectByType<Slider>();
    }

    public void SetMaxLucidity(float lucidity) 
    {
        slider.maxValue = lucidity;
        slider.value = lucidity;
    }

    public void SetLucidity(float lucidity)
    {
        slider.value = lucidity;
    }

}
