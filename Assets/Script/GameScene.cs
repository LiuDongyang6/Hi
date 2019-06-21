using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    float transbegin;
    Image cover;
    // Start is called before the first frame update
    void Start()
    {
        cover = GameObject.Find("Image").GetComponent<Image>();
        cover.enabled = false;
    }


    void Update()
    {

    }

    public void GoSelectScene()
    {
        SceneManager.LoadScene("SelectScene");
    }

    public void StopGame()
    {
        Debug.Log("stop!");
    }

    public void selectSOLO()
    {
        Toggle toggle = GameObject.Find("SOLO").GetComponent<Toggle>();
        if(toggle.isOn)
        {
            cover.enabled=true;
        }
        
    }

    public void Animationover()
    {
        Debug.Log("over!");
        cover.enabled = false;
    }
    public void SpeedChange_Up(){
        //anim["city"].speed=2f;
    }

    public void SpeedChang_Down(){
        //anim["city"].speed = 0.5f;
    }
}
