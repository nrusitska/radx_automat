using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;

namespace RadXAutomat.Audio
{
    public class AudioManager
    {
        static AudioManager _instance = new AudioManager();
        public static AudioManager Instance { get { return _instance; } }
        protected AudioManager() { }
        public void Test()
        {
            var synth = new SpeechSynthesizer();
            synth.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult);
            synth.SpeakSsml("<speak version='1.0' xml:lang='de'> <s>halli hallo</s> <s xml:lang='en'>Jack Bones </s></speak>");
        }
        WaveOut _output = new WaveOut();
        public void PlaySound(string sound, TimeSpan offset, TimeSpan length)
        {
            //            SoundPlayer player = new SoundPlayer();
            //            player.SoundLocation = path;
            //             player.Load();
            //             player.Play();

            _output.Stop();
            var file = new AudioFileReader(sound);
            var trimmer = new OffsetSampleProvider(file);
            trimmer.SkipOver = (offset);
            trimmer.Take = (length);
            _output.Init(trimmer);
            _output.Play();
            
        }

        public void PlaySound(string file, int waitBeforePlay, TimeSpan fileOffsetMillis, TimeSpan playDur)
        {
            Timer timer = null;
            timer = new Timer(
                (s)=> {
                    timer.Dispose();
                    PlaySound(file, fileOffsetMillis,playDur);
                    },
                null,waitBeforePlay,Timeout.Infinite);
            
        }
        
        public void PlaySweep(int offset, TimeSpan length)
        {
            PlaySound("RadSweep.wav",TimeSpan.MinValue,length);
        }
    }
}
