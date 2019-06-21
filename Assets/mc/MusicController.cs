using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
namespace HiMusic
{
    public abstract class MusicController
    {
        public MusicController()
        {

        }
        public static readonly string[] instrumentNames = {"guitar","keyboard","drums"};
        public static readonly string[] poseNames = { "ldynb" };
        public static readonly string[] effectNames = { "dj" };
        public static readonly string[] controlGesNames = { "up", "down", "pop" };
        abstract public void ReceiveGestureResults(IReadOnlyDictionary<string, double> discreteResults);
        protected Dictionary<string, GestureConfig> getInsConfigs()
        {
            string text = System.IO.File.OpenText("Assets/mc/InsConfig.json").ReadToEnd();
            var dic = JsonConvert.DeserializeObject<Dictionary<string, GestureConfig>>(text);
            return dic;
        }
        protected Dictionary<string, GestureConfig> getControlGesConfigs()
        {
            string text = System.IO.File.OpenText("Assets/mc/ControlGesConfig.json").ReadToEnd();
            var dic = JsonConvert.DeserializeObject<Dictionary<string, GestureConfig>>(text);
            return dic;
        }
        abstract public void start();
    }
    public class MusicControllerP1 : MusicController
    {
        public static MusicControllerP1 create()
        {
            return new MusicControllerP1();
        }

        private MusicControllerP1() :
            base()
        {
            instruments = new Dictionary<string, InstrumentP1>();
            var configs = getInsConfigs();
            foreach(var insName in instrumentNames)
            {
                GestureConfig config = configs[insName];
                instruments.Add(insName, InstrumentP1.Create(insName, config));
            }
        }

        public override void ReceiveGestureResults(IReadOnlyDictionary<string, double> results)
        {
            foreach (var ins in instruments)
            {
                ins.Value.Process(results[ins.Key]);
            }
        }
        public void quit()
        {
            foreach(var ins in instruments)
            {
                ins.Value.Stop();
            }
        }
        public override void start()
        {

        }
        Dictionary<string, InstrumentP1> instruments;
    }
    public class MusicControllerP2 : MusicController
    {
        public static MusicControllerP2 create()
        {
            return new MusicControllerP2();
        }
        MusicControllerP2() :
            base()
        {
            //instruments
            instruments = new Dictionary<string, InstrumentP2>();
            controlGestures = new Dictionary<string, ControlGes>();
            ldynb = InterruptiveIns.Create("exa",this);
            //ldynb = new 
            //Instrument
            {
                var InsConfigs = getInsConfigs();
                foreach (var insName in instrumentNames)
                {
                    GestureConfig config = InsConfigs[insName];
                    instruments.Add(insName, InstrumentP2.Create(insName, config, this));
                }
            }
            //control ges

            var GesConfigs = getControlGesConfigs();
            foreach (var gesName in controlGesNames)
            {
                GestureConfig config = GesConfigs[gesName];
                controlGestures.Add(gesName, new ControlGes(gesName, config, this));
            }

            //
            playedInstruments = new List<InstrumentP2>();
            //background player
            var backgroundP2 = new GameObject("backgroundP2");
            backgroundPlayer = backgroundP2.AddComponent<AudioSource>();
            backgroundPlayer.playOnAwake = false;
            //solo
            solo = new SoloController();
        }
        override public void ReceiveGestureResults(IReadOnlyDictionary<string, double> gestureResults)
        {
            ProcessResults(gestureResults);
        }
        public void ReceiveColorImageResult(string result)
        {
            ProcessColorResult(result);
        }
        public override void start()
        {
            CastIntro();
            this.ProcessResults += introProcessResults;
            this.ProcessColorResult += solo.Process;
        }
        private void CastIntro()
        {
            //cast back track
            backgroundPlayer.clip = Resources.Load<AudioClip>("Music/introP2");
            backgroundPlayer.loop = true;
            backgroundPlayer.Play();
        }
        private void introProcessResults(IReadOnlyDictionary<string, double> Results)
        {
            foreach(var ins in instruments)
            {
                if (Results.ContainsKey(ins.Key))
                {
                    ins.Value.Record(Results[ins.Key]);
                    //first action breaking ice
                    if (ins.Value.IsValid(true))
                    {
                        //stop playing intro
                        //
                        backgroundPlayer.Stop();
                        backgroundPlayer.loop = false;
                        //play backtrack
                        backgroundPlayer.clip = Resources.Load<AudioClip>("Music/" + "backgroundP2");
                        backgroundPlayer.Play();
                        //playSolo(default mute)
                        solo.Play();
                        //change processor
                        this.ProcessResults -= this.introProcessResults;
                        this.ProcessResults += this.bodyProcessResults;
                        //play the instrument
                        if (playedInstruments.Count == 0)
                            playedInstruments.Add(ins.Value);
                        ins.Value.setPosition(backgroundPlayer.time);
                        ins.Value.Play();
                        //
                        break;
                    }
                }
            }
        }
        private void bodyProcessResults(IReadOnlyDictionary<string,double> results)
        {
            foreach (var ins in playedInstruments)
            {
                ins.Play();
            }
            solo.Play();
            if (!backgroundPlayer.isPlaying)
                backgroundPlayer.Play();

            foreach (var ins in instruments)
            {
                if (results.ContainsKey(ins.Key))
                {
                    ins.Value.Process(results[ins.Key]);
                }
            }
            foreach (var ges in controlGestures)
            {
                if (results.ContainsKey(ges.Key))
                {
                    ges.Value.Process(results[ges.Key]);
                }
            }
            if(results.ContainsKey("exa"))
            {
                ldynb.Process(results["exa"]);
            }
        }
        public float getCurrentPosition()
        {
            return backgroundPlayer.time;
        }
        public List<InstrumentP2> playedInstruments { get; set; }
        public void pauseAllIns()
        {
            foreach (var ins in playedInstruments)
            {
                ins.sp.Pause();
            }
            backgroundPlayer.Pause();
            solo.Pause();
        }
        public void resumeAllIns()
        {
            foreach (var ins in playedInstruments)
            {
                ins.Play();
            }
            backgroundPlayer.Play();
            solo.Play();
        }
        private Dictionary<string, InstrumentP2> instruments;
        private Dictionary<string, ControlGes> controlGestures;
        private InterruptiveIns ldynb;
        private Action<IReadOnlyDictionary<string, double>> ProcessResults;
        private Action<string> ProcessColorResult;
        private AudioSource backgroundPlayer;
        private SoloController solo;
    }
}
