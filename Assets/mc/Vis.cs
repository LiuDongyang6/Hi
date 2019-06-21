using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vis : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        HiMusic.MusicDirector.getInstance().setCurrentPattern(2);
        HiMusic.MusicDirector.getInstance().startPlaying();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
