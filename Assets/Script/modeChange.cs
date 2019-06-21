using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class modeChange : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    void Start()
    {
        GameObject btnObj = GameObject.Find("Button_mode1");
        Button btn = btnObj.GetComponent<Button>();
        btn.onClick.AddListener(delegate ()
        {
            this.GoGameScene();
        });
        GameObject btnObj2 = GameObject.Find("Button_mode2");
        Button btn2 = btnObj2.GetComponent<Button>();
        btn2.onClick.AddListener(delegate ()
        {
            this.GoGameScene2();
        });
        HiMusic.MusicDirector.getInstance().setCurrentPattern(3);
        HiMusic.MusicDirector.getInstance().startPlaying();
    }

    Image icon;
    Sprite sp;
    void showPic(string buttonname)
    {
        icon = GameObject.Find("Image_mode").GetComponent<Image>();
        switch (buttonname)
        {
            case "Button_mode1":
                sp= Resources.Load("image/mode1_city", typeof(Sprite)) as Sprite;
                icon.overrideSprite = sp;
                break;
            case "Button_mode2":
                sp = Resources.Load("image/mode3_univers", typeof(Sprite)) as Sprite;
                icon.overrideSprite = sp;
                break;
            case "Button_mode3":
                sp = Resources.Load("image/mode2_forest", typeof(Sprite)) as Sprite;
                icon.overrideSprite = sp;
                break;
        }
    }
    
    void hidePic(string buttonname)
    {
        icon = GameObject.Find("Image_mode").GetComponent<Image>();
        sp = Resources.Load("image/mode_cover", typeof(Sprite)) as Sprite;
        icon.overrideSprite = sp;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //当鼠标进入button范围 name即为button名称
        showPic(name);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        //当鼠标移出button范围 name即为button名称
        hidePic(name);
    }

    public void GoGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void GoGameScene2()
    {
        Debug.Log("GameScene2");
        SceneManager.LoadScene("GameScene2");
    }



    // Update is called once per frame
    void Update()
    {
        if(HiMusic.MusicDirector.getInstance().result!=null)
        {

            var result = HiMusic.MusicDirector.getInstance().result;
            if(result == "Thumb_down")
            {
                GameObject kinect = GameObject.Find("kinect");
                Destroy(kinect);
                Invoke("GoGameScene1", 2);
                //GoGameScene();
            }
            else if(result == "Rock")
            {
                GameObject kinect = GameObject.Find("kinect");
                Destroy(kinect);
                Invoke("GoGameScene2", 2);
                //GoGameScene2();
            }
        }
    }
}
