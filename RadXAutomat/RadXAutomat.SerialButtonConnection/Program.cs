using RadXAutomat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace RadXAutomat.SerialButtonConnection
{
    class Program
    {
        /*
0	ESC
1	Lock 1
2	Lock 2
3	Klapp 1
4	Klapp 2
5	Klapp 3
6	Rechts
7	A
8	Unten
9	B
10	Links
11	C
12	Oben
13	E

            */
        private static Dictionary<int, int> _keyMap;
        private static void SetKeymap()
        {
            _keyMap = new Dictionary<int, int>();
            _keyMap[0]  = KeyConstants.GAME_CLEAR;
            _keyMap[1]  = KeyConstants.LOCK_1;
            _keyMap[2]  = KeyConstants.LOCK_2;
            _keyMap[3]  = KeyConstants.GAME_1;
            _keyMap[4]  = KeyConstants.GAME_2;
            _keyMap[5]  = KeyConstants.GAME_3;
            _keyMap[6]  = KeyConstants.RIGHT;
            _keyMap[7]  = KeyConstants.FUNC_A;
            _keyMap[8]  = KeyConstants.DOWN;
            _keyMap[9]  = KeyConstants.FUNC_B;
            _keyMap[10] = KeyConstants.LEFT;
            _keyMap[11] = KeyConstants.FUNC_C;
            _keyMap[12] = KeyConstants.UP;
            _keyMap[13] = KeyConstants.FUNC_COIN_RAD;

            foreach(var pair in _keyMap)
            {
                Console.WriteLine(pair.Key + ", " + ((Key)pair.Value));
            }
        }

        static void Main(string[] args)
        {
            SetKeymap();
            var con = new SerialButtonConnector("COM3");

            con.ButtonChanged += Con_ButtonChanged;
            con.BeginListening();

        }

        private static void Con_ButtonChanged(int button, bool state)
        {
            //Console.WriteLine(button + ": " + state);

            uint winstate = (uint)(state ? 0 : 2);//0==key down, 2 == key-up

            var key = _keyMap[button];

            if(key == KeyConstants.GAME_1 || key == KeyConstants.GAME_3|| key == KeyConstants.GAME_3)
            {
                //Spiel-Taste An => Up + Down für die Taste senden. Aus => Up+Down für ESC senden.
                if (!state) 
                    key = KeyConstants.GAME_CLEAR;

                SendKey((Key)key, true);
                Thread.Sleep(5);
                SendKey((Key)key, false);
            }
            else
            {
                SendKey((Key)key, state);
            }

            // keybd_event((byte)KeyInterop.VirtualKeyFromKey(key), 0, winstate, 0);
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        private static void SendKey(Key key, bool state)
        {
            Console.WriteLine(key.ToString() + " : " + state);
            uint winstate = (uint)(state ? 0 : 2);//0==key down, 2 == key-up
            keybd_event((byte)KeyInterop.VirtualKeyFromKey((Key)key), 0, winstate, 0);            
        }
    }
}
