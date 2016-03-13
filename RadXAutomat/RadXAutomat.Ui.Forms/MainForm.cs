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
    public partial class MainForm : Form
    {
        const int FlashAnimationDur = 3;
        const int ScrollAnimationDur = 7;
        const int ScrollPixelSteps = 175;

        public MainForm()
        {
            InitializeComponent();
            string source = File.ReadAllText("kernel.resource.c.txt");
            string cleaned = Regex.Replace(source, @"/\*(.*?)\*/", "", RegexOptions.Singleline);
            hackTextPanel.Text = cleaned;
            hackPanel.AutoScrollPosition = new Point();
            hackPanel.VerticalScroll.Maximum = hackTextPanel.PreferredHeight;
            hackPanel.VerticalScroll.Visible = false;
            KeyPress += MainForm_KeyPress;

        }
        void StartupAnimations()
        {
             DoOpacityAnimation();
             DoCodeAnimation();
//             DisplayContent();
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            
            var thr = new Thread(StartupAnimations);
            thr.Start();
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            Keys.   
        }

        void DoOpacityAnimation()
        {
            var rnd = new Random();
            var dur = TimeSpan.FromSeconds(FlashAnimationDur);
            var animationEnd = DateTime.Now + dur;
            while (DateTime.Now < animationEnd)
            {
                var rest = (animationEnd - DateTime.Now);
                double perc = (dur.Ticks - rest.Ticks) / (double)dur.Ticks;
                int number = rnd.Next((int)(perc * 100));
                if (number > 10)
                {
                    Invoke(new Action(() =>
                    {
                        hackPanel.Visible = true;

                    }));

                }
                else
                    Invoke(new Action(() =>
                    {
                        hackPanel.Visible = false;
                    }));
                Thread.Sleep(52);
            }
            Invoke(new Action(() =>
            {
                hackPanel.BackgroundImage.Dispose();
                hackPanel.BackgroundImage = null;
                hackPanel.BackColor = Color.Black;
            }));
        }

        void DisplayContent()
        {
            Invoke(new Action(() =>
            {
                hackPanel.Visible = false;
            }));
        }

        void DoCodeAnimation()
        {
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(ScrollAnimationDur);
            while (hackPanel.VerticalScroll.Value < hackPanel.VerticalScroll.Maximum && DateTime.Now < end)
            {
                Invoke(new Action(() =>
                {
                    hackPanel.VerticalScroll.Value = Math.Min(hackPanel.VerticalScroll.Maximum, hackPanel.VerticalScroll.Value + ScrollPixelSteps);                    
                }));
                Thread.Sleep(10);
            }


        }
    }
}
