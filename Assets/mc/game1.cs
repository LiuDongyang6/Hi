using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        HiMusic.MusicDirector.getInstance().setCurrentPattern(1);
        HiMusic.MusicDirector.getInstance().startPlaying();
    }

    // Update is called once per frame
    void Update()
    {

    }
}