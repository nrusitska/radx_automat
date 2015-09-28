using RadXAutomat.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RadXAutomat.Main
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Contains("-c"))
            {
                DataManager.GetInstance().BuildDatabase();
                return;
            }
            DataManager.GetInstance().Init();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
