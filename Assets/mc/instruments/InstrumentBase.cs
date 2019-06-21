using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace HiMusic
{
    public abstract class gestureBase
    {
        protected gestureBase(string gestureName, GestureConfig config)
        {
            threshold = config.threshold;
            backSwing = config.backSwing;
            name = gestureName;
            lastDetection = new DateTime();
        }
        /// <summary>
        /// return true if the instrument has accumulated enough frames to trigger playback
        /// </summary>
        /// <param name="clear">clear the accumulation record when returning true</param>
        /// <returns></returns>
        public bool IsValid(bool clear = true)
        {
            bool result = accumulation >= backSwing;
            if (result && clear)
            {
                ClearRecord();
            }
            return result;
        }
        /// <summary>
        /// should be called before activation
        /// </summary>
        /// <param name="confidence"></param>
        public void Record(double confidence)
        {
            if (confidence > threshold)
            {
                accumulation++;
            }
            else
            {
                accumulation = accumulation > 0 ? accumulation - 1 : 0;
            }
        }
        public void ClearRecord()
        {
            accumulation = 0;
        }
        public abstract void Process(double confidence);
        private double threshold;
        private int accumulation;
        /// <summary>
        /// how many valid frames should the gesture accumalate before activated
        /// </summary>
        private int backSwing;
        protected string name;
        protected DateTime lastDetection;
    }
    public abstract class InstrumentBase:gestureBase
    {
        protected InstrumentBase(string insName, GestureConfig config)
            : base(insName, config)
        {

        }
        public void Stop()
        {
            sp.Stop();
        }
        public void unpause()
        {
            sp.UnPause();
        }
        ///returns the soundPlayer
        public AudioSource sp { get; set; }
    }

    public abstract class InstrumentP1:InstrumentBase
    {
        public static InstrumentP1 Create(string insName, GestureConfig config)
        {
            if (config.isDiscrete)
                return new InstrumentP1Dis(insName, config);
            else
                return new InstrumentP1Con(insName, config);
        }
        protected InstrumentP1(string insName, GestureConfig config)
            : base(insName, config)
        {
            var obj = new GameObject(insName + "P1");
            sp = obj.AddComponent<AudioSource>();
            sp.clip = Resources.Load<AudioClip>("Music/" + insName + "P1");
            sp.playOnAwake = false;
            //delay
            delay = config.delay;
        }
        /// <summary>
        /// for discrete gestures:
        /// the minimum interval between two rings
        /// for continuous gestures:
        /// how many secondes the gesture may last after gesture ends
        /// </summary>
        protected double delay;
    }
    public class InstrumentP1Dis : InstrumentP1
    {

        public InstrumentP1Dis(string insName, GestureConfig config) : base(insName, config)
        {

        }
        public override void Process(double confidence)
        {
            Record(confidence);
            if (IsValid(true))
            {
                if ((DateTime.Now - base.lastDetection).TotalMilliseconds > delay)
                {
                    base.lastDetection = DateTime.Now;
                    sp.Play();
                    
                }
            }
        }
    }
    public class InstrumentP1Con : InstrumentP1
    {
        public InstrumentP1Con(string insName, GestureConfig config) : base(insName, config)
        {
            isRinging = false;
            runningThreshold = config.runningThreshold;
            sp.loop = true;
        }
        public override void Process(double confidence)
        {
            if (isRinging)
            {
                if (confidence > runningThreshold)
                {
                    lastDetection = DateTime.Now;
                }
                else
                {
                    if ((DateTime.Now - lastDetection).TotalMilliseconds > delay)
                    {
                        isRinging = false;
                        sp.Stop();
                    }
                }
            }
            else
            {

                Record(confidence);
                if (IsValid())
                {
                    sp.Play();
                    isRinging = true;
                    lastDetection = DateTime.Now;
                }
            }
        }
        private void LoopRePlay(object sender, EventArgs e)
        {
            sp.Play();
        }
        protected double runningThreshold;
        private bool isRinging;
    }

    public class InstrumentP2 : InstrumentBase
    {
        public static InstrumentP2 Create(string insName,GestureConfig config,MusicControllerP2 controller)
        {
            return new InstrumentP2(insName, config, controller);
        }
        protected InstrumentP2(string insName, GestureConfig config,MusicControllerP2 mc)
            : base(insName, config)
        {
            var obj = new GameObject(insName + "P2");
            sp = obj.AddComponent<AudioSource>();
            sp.clip = Resources.Load<AudioClip>("Music/" + insName + "P2");
            sp.playOnAwake = false;
            controller = mc;
        }
        public override void Process(double confidence)
        {
            if (controller.playedInstruments.Count() == 0 || this != controller.playedInstruments.Last())
            {
                Record(confidence);
                if (IsValid())
                {
                    if (!controller.playedInstruments.Contains(this))
                    {
                        setPosition(controller.getCurrentPosition());
                        Play();
                    }
                    else
                    {
                        controller.playedInstruments.Remove(this);
                    }
                    controller.playedInstruments.Add(this);
                    //
                }
            }
        }
        virtual public void Play()
        {
            if (!sp.isPlaying)
                sp.Play();
        }
        public void AlterVolume(float quantity)
        {
            sp.volume += quantity;
        }
        public void AlterSpeed(float ratio)
        {
            sp.pitch = ratio;
        }
        public void Pause(float delay = 0)
        {
            sp.Pause();
            if (delay != 0)
                sp.PlayDelayed(delay);
        }
        public void setPosition(float position)
        {
            sp.time = position;
        }
        public float currentPosition
        {
            get
            {
                return sp.time;
            }
        }
        private MusicControllerP2 controller;
    }

    public class SoloController
    {
        public SoloController()
        {
            var obj = new GameObject("solo" + "P2");
            sp = obj.AddComponent<AudioSource>();
            sp.clip = Resources.Load<AudioClip>("Music/" + "solo" + "P2");
            sp.playOnAwake = false;
            sp.mute = false;
        }

        public void Process(string result)
        {
            if (result == "Thumb_down")
            {
                sp.mute = false;
            }
            else if (result == "Rock")
            {
                sp.mute = true;
            }
        }

        public void setMute(bool isMute)
        {
            if (isMute)
                sp.mute = true;
            else
                sp.mute = false;
        }

        public void Play()
        {
            if(!sp.isPlaying)
            sp.Play();
        }
        public void Pause()
        {
            sp.Pause();
        }
        public void unPause()
        {
            sp.UnPause();
        }
        private AudioSource sp { get; set; }
    }

    public class InterruptiveIns : InstrumentBase
    {
        public static InterruptiveIns Create(string insName, MusicControllerP2 controller)
        {
            GestureConfig config = new GestureConfig();
            config.threshold = 0.2;
            config.backSwing = 1;
            config.delay = 10000;
            return new InterruptiveIns(insName, config, controller);
        }
        protected InterruptiveIns(string insName, GestureConfig config, MusicControllerP2 mc)
            : base(insName, config)
        {
            var obj = new GameObject(insName + "P2");
            sp = obj.AddComponent<AudioSource>();
            sp.clip = Resources.Load<AudioClip>("Music/" + insName + "P2");
            sp.playOnAwake = false;
            controller = mc;
        }
        public InterruptiveIns(string insName, MusicControllerP2 mc):base(insName, new GestureConfig())
        {
            
            var obj = new GameObject(insName + "P2");
            sp = obj.AddComponent<AudioSource>();
            sp.clip = Resources.Load<AudioClip>("Music/" + insName + "P2");
            sp.playOnAwake = false;
            controller = mc;
        }
        public override void Process(double confidence)
        {
            Record(confidence);
            if (IsValid())
            {
                if ((DateTime.Now - lastDetection).TotalMilliseconds > 10000)
                {
                    lastDetection = DateTime.Now;
                    MusicDirector.getInstance().pauseReceiving();
                    controller.pauseAllIns();
                    sp.Play();
                    System.Timers.Timer t = new System.Timers.Timer(sp.clip.length * 1000);//实例化Timer类，设置时间间隔
                    t.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e)
                    {
                        MusicDirector.getInstance().resumeReceiving();
                    };//到达时间的时候执行事件
                    t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)
                    t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件
                }
            }
        }
        private MusicControllerP2 controller;
    }

    public class ControlGes : gestureBase
    {
        public ControlGes(string gestureName, GestureConfig config, MusicControllerP2 musicController)
            : base(gestureName, config)
        {
            controller = musicController;
            delay = config.delay;
        }
        override public void Process(double confidence)
        {
            Record(confidence);
            if (IsValid(true))
            {
                if ((DateTime.Now - lastDetection).TotalMilliseconds > delay)
                {
                    lastDetection = DateTime.Now;
                    act();
                }
            }
        }
        public void act()
        {
            if(name == "pop")
            {
                if(controller.playedInstruments.Count()!=0)
                {
                    controller.playedInstruments.Last().Stop();
                    controller.playedInstruments.RemoveAt(controller.playedInstruments.Count() - 1);              
                }
            }
            else if(name == "up")
            {
                if (controller.playedInstruments.Count() != 0)
                {
                    controller.playedInstruments.Last().AlterVolume(0.5f);
                }
            }
            else if(name == "down")
            {
                controller.playedInstruments.Last().AlterVolume(-0.5f);
            }
        }
        double delay;
        private MusicControllerP2 controller;
    }

    public class GestureConfig
    {
        public double threshold;
        public double runningThreshold;
        public double delay;
        /// <summary>
        /// is discrete?
        /// </summary>
        public bool isDiscrete;
        public int backSwing;
    }
}
