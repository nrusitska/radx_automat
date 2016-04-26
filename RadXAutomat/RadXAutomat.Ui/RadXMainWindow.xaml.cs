using RadXAutomat.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RadXAutomat.Ui
{
    /// <summary>
    /// Interaktionslogik für RadXMainWindow.xaml
    /// </summary>
    public partial class RadXMainWindow : Window
    {        
        public RadXMainWindow()
        {
            InitializeComponent();
        }

        private void TypewriteTextblock(string textToAnimate, TextBlock txt, TimeSpan timeSpan)
        {
            Storyboard story = new Storyboard();
            story.FillBehavior = FillBehavior.HoldEnd;
            //story.RepeatBehavior = RepeatBehavior.Forever;

            DiscreteStringKeyFrame discreteStringKeyFrame;
            StringAnimationUsingKeyFrames stringAnimationUsingKeyFrames = new StringAnimationUsingKeyFrames();
            stringAnimationUsingKeyFrames.Duration = new Duration(timeSpan);

            string tmp = string.Empty;
            foreach (char c in textToAnimate)
            {
                discreteStringKeyFrame = new DiscreteStringKeyFrame();
                discreteStringKeyFrame.KeyTime = KeyTime.Paced;
                tmp += c;
                discreteStringKeyFrame.Value = tmp;
                stringAnimationUsingKeyFrames.KeyFrames.Add(discreteStringKeyFrame);
            }
            Storyboard.SetTargetName(stringAnimationUsingKeyFrames, txt.Name);
            Storyboard.SetTargetProperty(stringAnimationUsingKeyFrames, new PropertyPath(TextBlock.TextProperty));
            story.Children.Add(stringAnimationUsingKeyFrames);

            story.Begin(txt);
        }
        void Write(string text)
        {
            double msPerChar = 12.5;
            _consoleContent.Text = null;
            var dur = TimeSpan.FromMilliseconds(msPerChar * text.Length);
            TypewriteTextblock(text, _consoleContent, dur);
        }
        RadXInteractionModel _model;

        private double AnimateRadBar(int rads)
        {
            if (rads == -1)
                return 0;
//             rads = (int)Math.Min(rads, _radProgrssBar.Maximum);
//             rads = (int)Math.Max(rads, _radProgrssBar.Minimum);
            var milisPerRad = 12.5;
            //_radProgrssBar.ClearValue(Slider.ValueProperty);
            _radProgrssBar.Value = 0;// _radProgrssBar.Maximum;
            _radProgrssBar.Opacity = 1;
            var time = TimeSpan.FromMilliseconds(milisPerRad * Math.Min(rads,300));
            var value = rads;//_radProgrssBar.Maximum - rads;
            var dur = new Duration(time);
            var ani = new DoubleAnimationUsingKeyFrames();
            ani.Duration = dur;
            ani.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))));
            ani.KeyFrames.Add(new LinearDoubleKeyFrame(value, KeyTime.FromTimeSpan(time)));
            _radProgrssBar.BeginAnimation(Slider.ValueProperty, ani, HandoffBehavior.SnapshotAndReplace);



            var opaqAni = new DoubleAnimationUsingKeyFrames();

            opaqAni.KeyFrames.Add(new DiscreteDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))));
            int flOfs = 200;
            opaqAni.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromTimeSpan(time.Add(TimeSpan.FromMilliseconds(flOfs)))));
            opaqAni.KeyFrames.Add(new DiscreteDoubleKeyFrame(1, KeyTime.FromTimeSpan(time.Add(TimeSpan.FromMilliseconds(flOfs+300)))));
            opaqAni.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromTimeSpan(time.Add(TimeSpan.FromMilliseconds(flOfs+600)))));
            opaqAni.KeyFrames.Add(new DiscreteDoubleKeyFrame(1, KeyTime.FromTimeSpan(time.Add(TimeSpan.FromMilliseconds(flOfs+900)))));
            opaqAni.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromTimeSpan(time.Add(TimeSpan.FromMilliseconds(flOfs+1900)))));

            _radProgrssBar.BeginAnimation(Slider.OpacityProperty, opaqAni, HandoffBehavior.SnapshotAndReplace);
            var sleeptime = time.TotalMilliseconds + flOfs + 1900;

            AudioManager.Instance.PlaySound("RadSweep.wav", TimeSpan.FromMilliseconds(0), time);
            AudioManager.Instance.PlaySound("tripple-bell.wav", flOfs + (int)time.TotalMilliseconds,TimeSpan.FromMilliseconds(0), TimeSpan.MaxValue);

            return sleeptime;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            AudioManager.Instance.SetDispatcher(Dispatcher);
            //Write("Als Gregor Samsa eines Morgens aus unruhigen Träumen erwachte, fand er sich in seinem Bett zu einem ungeheueren Ungeziefer verwandelt. ");
            StartCursorAnimation();
            _model = new RadXInteractionModel();
            _model.Write = (string text) => {
                Dispatcher.BeginInvoke(new Action(()=>{ Write(text); }));
            };
            _model.WriteInput = (string text) =>
            {
                Dispatcher.BeginInvoke(new Action(() => { _consoleInputContent.Text = text; }));
            };
            _model.ShowRadsCountAnimation = (int rads) =>
            {
                double aniDur = 0;
                Dispatcher.Invoke(new Action(() => { aniDur = AnimateRadBar(rads); }));
                return aniDur;
            };
            _model.Start();
            
        }
        void StartCursorAnimation()
        {            
            var ani = new StringAnimationUsingKeyFrames() {  Duration=new Duration(TimeSpan.FromMilliseconds(1000)) };
            ani.KeyFrames.Add(new DiscreteStringKeyFrame() { Value = "", KeyTime=KeyTime.FromPercent(0) });
            ani.KeyFrames.Add(new DiscreteStringKeyFrame() { Value = " ", KeyTime = KeyTime.FromPercent(0.5) });

            ani.RepeatBehavior = RepeatBehavior.Forever;
            _consolePostCursor.BeginAnimation(TextBlock.TextProperty, ani);
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if(_model != null)
            {
                _model.DoInput(e.Key, false);
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (_model != null)
            {
                _model.DoInput(e.Key, true);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_model != null)
            {
                try
                {
                    _model.Dispose();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
        }
    }
}
