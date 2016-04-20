using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RadXAutomat.Ui
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        const int FlashAnimationDur = 3;
        const int ScrollAnimationDur = 7;
        const int ScrollPixelSteps = 100;
        public StartupWindow()
        {
            InitializeComponent();
            string source = File.ReadAllText("kernel.resource.c.txt");
            string cleaned = Regex.Replace(source, @"/\*(.*?)\*/", "",RegexOptions.Singleline);
            _codeBlock.Text = cleaned;
        }

        void StartupAnimations()
        {
            DoOpacityAnimation();
            DoCodeAnimation();
            DisplayContent();
        }

        void DoOpacityAnimation()
        {
            var rnd = new Random();
            var dur = TimeSpan.FromSeconds(FlashAnimationDur);
            var animationEnd = DateTime.Now + dur;
            while (DateTime.Now < animationEnd)
            {
                var rest = ( animationEnd - DateTime.Now);
                double perc = (dur.Ticks - rest.Ticks) / (double)dur.Ticks;
                int number = rnd.Next((int) (perc * 100));
                if (number > 10)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        _codeAnimationArea.Opacity = 1;
                    }));

                }
                else
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        _codeAnimationArea.Opacity = 0;
                    }));
                Thread.Sleep(52);
            }
            Dispatcher.BeginInvoke(new Action(() => {
                _codeAnimationArea.OpacityMask = null;
                _codeAnimationArea.Opacity = 1;
            }));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //StartCodeAnimation();
            var thr = new Thread(StartupAnimations);
            thr.Start();
            
        }

        void DisplayContent()
        {
            Dispatcher.BeginInvoke(new Action(() => {
                //_content.Visibility = Visibility.Visible;
                _codeAnimationArea.Visibility = Visibility.Hidden;
                var wnd = new RadXMainWindow() { Owner = this };
                wnd.ShowDialog();
            }));
        }

        void DoCodeAnimation()
        {
            _codeAnimationArea.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _codeAnimationArea.Background = Brushes.Black;
                }));
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(ScrollAnimationDur);
            while (_scroller.VerticalOffset < _scroller.ScrollableHeight && DateTime.Now < end) 
            {
                _scroller.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _scroller.ScrollToVerticalOffset(_scroller.VerticalOffset + ScrollPixelSteps);
                }));
                Thread.Sleep(10);
            }
            

        }
    }
}
