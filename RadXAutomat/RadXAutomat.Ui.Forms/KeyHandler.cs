using RadXAutomat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RadXAutomat.Ui.Forms
{
    public class KeyHandler
    {
        private RadXMainForm _mainForm;
        public KeyHandler(RadXMainForm form)
        {
            _mainForm = form;
        }

        public void HandleKey(int key)
        {
            switch (key)
            {
                case KeyConstants.GAME_1:
                    if (IsBatteryMode())
                        MessageBox.Show("Not Available in Battery Mode");
                    break;
            }
        }

        private bool IsBatteryMode()
        {
            return SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Offline;
        }
    }
}
