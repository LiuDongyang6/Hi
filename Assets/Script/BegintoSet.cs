using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BegintoSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject btnObj = GameObject.Find("Setting");
        Button btn = btnObj.GetComponent<Button>();
        btn.onClick.AddListener(
            delegate ()
            {
                this.GoSettingsScene(btnObj);
            }
        );
    }

    public void GoSettingsScene(GameObject NScene)
    {

        SceneManager.LoadScene("SettingScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
