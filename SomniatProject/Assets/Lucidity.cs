using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lucidity : MonoBehaviour
{

    public Slider slider;

    public void SetMaxLucidity(int lucidity) 
    {
        slider.maxValue = lucidity;
        slider.value = lucidity;
    }

    public void SetLucidity(int lucidity)
    {
        slider.value = lucidity;
    }

}
