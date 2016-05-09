using RadXAutomat.Data;
using RadXAutomat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RadXAutomat.Main
{
    public partial class Form1 : Form
    {
        ArcadeInteractionManager _manager = new ArcadeInteractionManager();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _manager.StartGame1();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _manager.StartGame2();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _manager.StartGame3();
        }
    }
}
