using RadXAutomat.NfcDongle;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RadXAutomat.TestUi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _nfcWrapper = new NfcDongleWrapper();
            _nfcWrapper.TagFound += _nfcWrapper_TagFound;
            _nfcWrapper.BeginSearch();
        }

        private void _nfcWrapper_TagFound(object sender, string id)
        {
            BeginInvoke(new Action(()=> {

                tagIdLabel.Text = "Tag: " + id;
                dongleGroupBox.Enabled = false;
                radsLabel.Text = "";
                if(id != null)
                {
                    try
                    {
                        dongleGroupBox.Enabled = true;
                        radsLabel.Text = "Rads: " + _nfcWrapper.GetRads();
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                        MessageBox.Show(ex.Message);
                    }
                }
            }));
        }

        NfcDongleWrapper _nfcWrapper;

    }
}
