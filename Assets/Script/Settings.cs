using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public int BasicMusic = 30;              //声音设置为1~100
    public int MoveSpeed = 2;                //速度设置 0.5x为1 1x为2

    // Start is called before the first frame update
    void Start()
    {
        MoveSpeed++;
        GameObject.DontDestroyOnLoad(gameObject);
    }

    public void setMusic()
    {
        Slider slider = GameObject.Find("Canvas/music_slider").GetComponent<Slider>();
        BasicMusic = (int)(slider.value * 100);
    }

    public void selectHalf()
    {
        Toggle toggle = GameObject.Find("half_toggle").GetComponent<Toggle>();
        if (toggle.isOn) {

           MoveSpeed = 1;
        }
     
    }

    public void selectOne()
    {
        Toggle toggle = GameObject.Find("one_toggle").GetComponent<Toggle>();
        if (toggle.isOn) {
            MoveSpeed = 2;
        }
    }

    public void selectTwo()
    {
        Toggle toggle = GameObject.Find("two_toggle").GetComponent<Toggle>();
        if (toggle.isOn) {
            MoveSpeed = 4;
        }
    }

    public int getBasicMusic()
    {
        return BasicMusic;
    }

    public int getMoveSpeed()
    {
        return MoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
