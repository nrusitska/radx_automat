using RadXAutomat.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace RadXAutomat.Model
{
    public class InteractionManager
    {
        Process _currentGame;
        public void KillCurrentGame()
        {
            if (_currentGame != null && !_currentGame.HasExited)
                _currentGame.Kill();
        }

        private void StartGame(string key)
        {
            KillCurrentGame();

            IniFile config = new IniFile(Path.GetFullPath("ArcadeCommands.ini"));
            var command = config.IniReadValue("Commands", key);
            var args = command.Split(' ');
            string cmd = args[0];
            string param = string.Join(" ", args.Skip(1));

            var startinf = new ProcessStartInfo(cmd, param);
            startinf.WorkingDirectory = Path.GetDirectoryName(cmd);

            _currentGame = Process.Start(startinf);
        }

        public void StartGame1() { StartGame("GAME1"); }
        public void StartGame2() { StartGame("GAME2"); }
        public void StartGame3() { StartGame("GAME3"); }
    }
}
