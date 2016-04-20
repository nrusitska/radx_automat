using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private void Window_ContentRendered(object sender, EventArgs e)
        {
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
                _model.DoInput(e.Key);
            }
        }
    }
}
