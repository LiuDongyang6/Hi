using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleControl : MonoBehaviour
{
    ToggleGroup group;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ShowToggle()
    {
        gameObject.SetActive(false);
    }

    public void HideToggle()
    {
        gameObject.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
