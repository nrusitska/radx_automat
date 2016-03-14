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
    public partial class RadXMainForm : Form
    {
        const int FlashAnimationDur = 3;
        const int ScrollAnimationDur = 7;
        const int ScrollPixelSteps = 175;

        private KeyHandler _keyHandler;

        public RadXMainForm()
        {
            InitializeComponent();
            _keyHandler = new KeyHandler(this);
            this.KeyUp += MainForm_KeyUp;
#if !DEBUG
            using (var startF = new StartForm())
            {
                startF.ShowDialog();
            }
#endif
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    tabControl.SelectedIndex = Math.Min(tabControl.SelectedIndex + 1, tabControl.TabCount);
                    return;
                case Keys.Left:
                    tabControl.SelectedIndex = Math.Min(tabControl.SelectedIndex - 1, tabControl.TabCount);
                    return;
            }
            _keyHandler.HandleKey((int)e.KeyCode);
        }


        private void MainForm_Shown(object sender, EventArgs e)
        {
            
           
        }        
    }
}
