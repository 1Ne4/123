using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stelthBar : MonoBehaviour
{
    public  Slider slider;
    
    // Start is called before the first frame update
    public void SetBar(int amountOfTime)
    {
        slider.value = amountOfTime;
    }
}
