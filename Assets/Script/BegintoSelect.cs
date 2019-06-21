using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BegintoSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject btnObj1 = GameObject.Find("Start");
        Button btn1 = btnObj1.GetComponent<Button>();
        btn1.onClick.AddListener(
            delegate () {
                this.GoSelectScene(btnObj1);
            }
        );

        GameObject btnObj2 = GameObject.Find("Button_back");
        Button btn2 = btnObj2.GetComponent<Button>();
        btn2.onClick.AddListener(
            delegate () {
                this.BackBeginScene(btnObj2);
            }
        );

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoSelectScene(GameObject NScene)
    {

        SceneManager.LoadScene("SelectScene");
    }

    public void BackBeginScene(GameObject NScene)
    {
        SceneManager.LoadScene("BeginScene");
    }

}
