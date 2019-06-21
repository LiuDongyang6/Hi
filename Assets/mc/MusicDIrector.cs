using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
namespace HiMusic
{
    class MusicDirector
    {
        public static MusicDirector getInstance()
        {
            if (instance == null)
            {
                instance = new MusicDirector();
                instance.init();
            }
            return instance;
                
        }
        public void setCurrentPattern(int pattern)
        {
            currentPattern = pattern;
        }
        public void setBackgroundVolume(int volume)
        {
            checkState();
        }
        public void setSpeedRatio(double ratio)
        {
            checkState();
        }
        private void checkState()
        {
            if (currentPattern == -1)
            {
                Debug.Log("pattern not set");
                throw new PatternSettingException("pattern not set");
            }
        }
            
        public void receiveGestureResults(Dictionary<string,double> results)
        {
            if (!isPlaying)
                return;
            if (currentPattern == 1)
            {
                controllerP1.ReceiveGestureResults(results);
            }
            else if (currentPattern == 2)
            {
                controllerP2.ReceiveGestureResults(results);
            }
        }
        public void receiveColorImage(Byte[] array)
        {
            if (currentPattern == 3)
            {
                result = colorRecognizer.recognize(array);
                colorImageReady = true;
            }
        }
        public void passColorResult()
        {
            controllerP2.ReceiveColorImageResult(result);
        }

        public void startPlaying()
        {
            checkState();
            if(currentPattern == 1)
            {
                controllerP1 = MusicControllerP1.create();
                controllerP2 = MusicControllerP2.create();
                controllerP1.start();
            }
            else if(currentPattern == 2)
            {
                controllerP2 = MusicControllerP2.create();
                controllerP2.start();
            }
            else if(currentPattern ==3)
            {
                colorImageReady = true;
            }
            isPlaying = true;
        }
        private void init()
        {
            colorRecognizer =  new GestureRecognition();
            currentPattern = -1;
            isPlaying = false;
            colorImageReady = true;
        }
        public void stopPlaying()
        {
            isPlaying = false;
        }
        public void pauseReceiving()
        {
            isPlaying = false;
        }
        public void resumeReceiving()
        {
            isPlaying = true;
        }
        public bool colorImageReady
        {
            get;set;
        }
        public string result;
        private int currentPattern;
        private static MusicDirector instance = null;
        private MusicControllerP1 controllerP1;
        private MusicControllerP2 controllerP2;
        private GestureRecognition colorRecognizer;
        private const int colorFrames = 5;
        private MusicDirector() { }
        private bool isPlaying;
    }

    class PatternSettingException : ApplicationException
    {
        public PatternSettingException(string msg) : base(msg)
        {


        }
    }
}
