using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

namespace RadXAutomat.SerialButtonConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            var con = new SerialButtonConnector("COM3");

            con.ButtonChanged += Con_ButtonChanged;
            con.BeginListening();

        }

        private static void Con_ButtonChanged(int button, bool state)
        {
            var key = Key.LWin;
            Console.WriteLine(button + ": " + state);

            uint winstate = (uint)(state ? 0 : 2);//0==key down, 2 == key-up
            keybd_event((byte)KeyInterop.VirtualKeyFromKey(key), 0, winstate, 0);
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
    }
}
