using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace RadXAutomat.Ui.Forms
{
    public partial class StartForm : Form
    {
        const int FlashAnimationDur = 3;
        const int ScrollAnimationDur = 7;
        const int ScrollPixelSteps = 175;

        public StartForm()
        {
            InitializeComponent();
            string source = File.ReadAllText("kernel.resource.c.txt");
            string cleaned = Regex.Replace(source, @"/\*(.*?)\*/", "", RegexOptions.Singleline);
            hackTextPanel.Text = cleaned;
            hackPanel.AutoScrollPosition = new Point();
            hackPanel.VerticalScroll.Maximum = hackTextPanel.PreferredHeight;
            hackPanel.VerticalScroll.Visible = false;
            hackPanel.Visible = false;            
        }

        void StartupAnimations()
        {
            DoOpacityAnimation();
            DoCodeAnimation();
            DisplayContent();
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            
            var thr = new Thread(StartupAnimations);
            thr.Start();
            //BeginInvoke(new Action(StartupAnimations));
        }

        void DoOpacityAnimation()
        {
            Invoke(new Action(() =>
            {
                hackPanel.Visible = true;
                hackPanel.Update();
            }));
            var rnd = new Random();
            var dur = TimeSpan.FromSeconds(FlashAnimationDur);
            var animationEnd = DateTime.Now + dur;
            var img = hackPanel.BackgroundImage;
            while (DateTime.Now < animationEnd)
            {
                var rest = (animationEnd - DateTime.Now);
                double perc = (dur.Ticks - rest.Ticks) / (double)dur.Ticks;
                int number = rnd.Next((int)(perc * 100));
                if (number > 10)
                {
                    Invoke(new Action(() =>
                    {
                        hackPanel.BackgroundImage = img;
                        hackPanel.Update();
                    }));

                }
                else
                    Invoke(new Action(() =>
                    {
                        hackPanel.BackgroundImage = null;
                        hackPanel.Update();
                    }));
                Thread.Sleep(52);
            }
            BeginInvoke(new Action(() =>
            {
                //hackPanel.BackgroundImage.Dispose();
                hackPanel.BackColor = Color.Black;
                hackPanel.BackgroundImage = null;
                img.Dispose();
                hackPanel.Update();
            }));
        }

        void DisplayContent()
        {
            Invoke(new Action(() =>
            {
                Close();
//                 var form = new RadXMainForm();
//                 form.Shown += (s,a) => { this.Close(); };
//                 BeginInvoke(new Action(()=>form.ShowDialog()));
//                 
            }));
        }

        void DoCodeAnimation()
        {
            Invoke(new Action(() =>
            {
                hackPanel.AutoScroll = true;
                hackPanel.VerticalScroll.Visible = false;
                hackPanel.PerformLayout();
            }));
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(ScrollAnimationDur);
            while (hackPanel.VerticalScroll.Value < hackPanel.VerticalScroll.Maximum && DateTime.Now < end)
            {
                Invoke(new Action(() =>
                {
                    hackPanel.VerticalScroll.Value = Math.Min(hackPanel.VerticalScroll.Maximum, hackPanel.VerticalScroll.Value + ScrollPixelSteps);
                    hackPanel.PerformLayout();
                }));
                Thread.Sleep(10);
            }


        }
    }
}
