using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToBegin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject btnObj = GameObject.Find("Button_back");
        Button btn = btnObj.GetComponent<Button>();
        btn.onClick.AddListener(
            delegate () {
                this.BackBeginScene(btnObj);
            }
        );
    }

    public void BackBeginScene(GameObject NScene)
    {
        SceneManager.LoadScene("BeginScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
