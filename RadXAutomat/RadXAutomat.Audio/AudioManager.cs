using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace RadXAutomat.Audio
{
    public class AudioManager
    {
        static AudioManager _instance = new AudioManager();
        public static AudioManager Instance { get { return _instance; } }
        public void SetDispatcher(Dispatcher dis) { _Dispatcher = dis; }
        protected Dispatcher _Dispatcher;
        protected AudioManager()
        {
//             var thr = new Thread(new ThreadStart(() =>
//             {
//                 _Dispatcher = Dispatcher.CurrentDispatcher;
                //_output = new WaveOut();
//                 Dispatcher.Run();
//             }))
//             { Name = "RadUI-Audio-Thread", Priority=ThreadPriority.AboveNormal };
//             thr.Start();
//             
//             while (_output == null || _Dispatcher == null)
//                 thr.Join(100);
        }

        WaveOut _output = new WaveOut();
        public void PlaySound(string sound, TimeSpan offset, TimeSpan length)
        {
            _Dispatcher.Invoke(new Action(() => {
                _output.Dispose();
                _output = new WaveOut();
            }));
            
            var ac = new Action(() =>
            {
                //WaveOut _output = new WaveOut();
                _output.Stop();                
                var file = new AudioFileReader("sounds\\" + sound);
                var trimmer = new OffsetSampleProvider(file);
                trimmer.SkipOver = (offset);
                if (length != TimeSpan.MaxValue)
                    trimmer.Take = (length);
                _output.Init(trimmer);
                _output.Play();
            });
            //{ Priority= ThreadPriority.Highest }.Start();
            _Dispatcher.BeginInvoke(ac);
            
        }
        public void PlaySound(string file)
        {
            PlaySound(file, TimeSpan.FromMilliseconds(0), TimeSpan.MaxValue);
        }
        public void PlaySound(string file, int waitBeforePlay)
        {
            PlaySound(file, waitBeforePlay, TimeSpan.FromMilliseconds(0), TimeSpan.MaxValue);
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

    }
}
