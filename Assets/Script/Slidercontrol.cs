using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slidercontrol : MonoBehaviour
{
    Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowSlider()
    {
        gameObject.SetActive(true);
        slider = gameObject.GetComponent<Slider>();
        slider.value = 0.3f;
    }

    public void HideSlider()
    {
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
