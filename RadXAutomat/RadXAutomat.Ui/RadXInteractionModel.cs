using RadXAutomat.Model;
using RadXAutomat.NfcDongle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace RadXAutomat.Ui
{
    public delegate void RadXWrite(string text);

    public class RadXInteractionModel : IDisposable
    {
        enum ModelState
        {
            loked, waiting, dongleReadySelectAction, waitingForTakeoff
        }
        enum CommandOptions
        {
            RadX=0,
            PureLive=1,
            RadAway=2,
            ReadRads=3
        }
        const string LockMessage = "Na dann beweis mir mal, dass Du hier was zu melden hast!";
        const string WelcomeMessage = "Alles klar Ödländer, dann leg mal Deine Hand auf, und wir schauen uns das mal an...";
        const string DongleFoundMessage = "Ah, da bist Du ja.";
        const string DongleFoundMessage_Jack = "Na das Händchen erkenn ich doch! Ladies and Gentleman - Jack Bones is in the house!";

        const string ShowOptions = "Dann lass uns mal loslegen. Wenn Du RadX nehmen willst, kann es gleich losgehen.\n"
            +"Sag mir mit dem RadX-Knopf, wie viele du nehmen willst.\n"
            +"Oder Du schaltest mit dem Joystick links oder rechts durch die Möglichkeiten. Du kannst [RadX nehmen] [PureLive nehmen] [RadAway nehmen] oder [Strahlung auslesen]\n"
            +"Mit [D] geht's dann los!";
        const string WaitForTakeoffMessage = "So das war's für Dich. Dann mal raus hier und mach Platz für den nächsten!";

        const string TakeRadXMessage = "Na dann mal rein mit dem guten Zeug!";
        const string ReadRadsMessage = "Dann wollen wir mal sehen, wie verstrahlt du wirklich bist...";
        const string RadResultMessage_Green = "Alle Achtung! So sauber, wie frisch aus dem Bunker!";
        const string RadResultMessage_Yellow = "Du solltest langsam mal aufpassen, wo Du so rumläufst";
        const string RadResultMessage_Red = "Oh oh. Das sieht böse aus. Vielleicht gehst Du einfach mal wo anders hin. Dort wo Du keinen anderen verstahlen kannst.";

        public RadXWrite Write { get; set; }
        public RadXWrite WriteInput { get; set; }
        public Func<int,double> ShowRadsCountAnimation { get; set; }

        private ModelState _state;
        private Action<int, bool> _currentStateHandleKey;
        private CommandOptions _currentCommand;
        private int _radXTakeCount = 0;
        private NfcDongleWrapper _dongleConnector;
        public RadXInteractionModel()
        {
            _dongleConnector = new NfcDongleWrapper();
            _dongleConnector.BeginSearch();
            _dongleConnector.TagFound += _dongleConnector_TagFound;
            _dongleConnector.TagLost += _dongleConnector_TagLost;
        }

        private void _dongleConnector_TagLost(object sender, EventArgs e)
        {
            if(_state == ModelState.waitingForTakeoff || _state == ModelState.dongleReadySelectAction)
                ChangeState_Waiting();
        }

        private void _dongleConnector_TagFound(object sender, string e)
        {
            if (_state == ModelState.waiting)
                ChangeState_ReadyForAction();
        }

        public void Start()
        {           
            ChangeState_Locked();

            //Demo();
        }

        public void DoInput(Key key, bool keyState)
        {
            var thread = new Thread(() =>
            {
                HandleKey_CheckIfLocked((int)key, keyState);
                if (_currentStateHandleKey != null)
                    _currentStateHandleKey((int)key, keyState);

            });
            thread.Start();

        }
        void Demo()
        {
            var thread = new Thread(() => {
                
                Thread.Sleep(4000);
                ChangeState_Waiting();
                Thread.Sleep(5000);
                ChangeState_ReadyForAction();


            });
            thread.Start();            
        }

        void ChangeState_Locked()
        {
            _state = ModelState.loked;
            _currentStateHandleKey = HandleKey_LockedWaitForUnlock;
            Write(LockMessage);
            lockKey1 = false;
            lockKey2 = false;
            lastLockTime = DateTime.MinValue;
        }
        bool lockKey1, lockKey2;
        DateTime lastLockTime = DateTime.MinValue;
        TimeSpan unlockTimeout = TimeSpan.FromMilliseconds(700);
        void HandleKey_LockedWaitForUnlock(int input, bool state)
        {
            if (input == KeyConstants.LOCK_1 && state)
            {
                lockKey1 = true;
                if (lockKey2 && DateTime.Now < lastLockTime + unlockTimeout)
                    ChangeState_Waiting();
                else if (!lockKey2)
                    lastLockTime = DateTime.Now;
            }
            else if (input == KeyConstants.LOCK_2 && state)
            {
                lockKey2 = true;
                if (lockKey1 && DateTime.Now < lastLockTime + unlockTimeout)
                    ChangeState_Waiting();
                else if(state)
                    lastLockTime = DateTime.Now;
            }
        }

        void HandleKey_CheckIfLocked(int input, bool state)
        {
            if ( !state 
                && (input == KeyConstants.LOCK_1 || input == KeyConstants.LOCK_2))
                ChangeState_Locked();
        }

        void ChangeState_Waiting()
        {
            _state = ModelState.waiting;
            _currentStateHandleKey = null;
            Write(WelcomeMessage);
        }
        void ChangeState_WaitForTakeoff()
        {
            _state = ModelState.waitingForTakeoff;
            _currentStateHandleKey = null;
            Write(WaitForTakeoffMessage);
        }

        void ChangeState_ReadyForAction()
        {
            _radXTakeCount = 2;
            _state = ModelState.dongleReadySelectAction;
            _currentStateHandleKey = HandleKey_SelectAction;
            Write(DongleFoundMessage);
            Thread.Sleep(5000);
            Write(ShowOptions);
            HandleKey_SelectAction(int.MinValue, false);
        }
        void HandleKey_SelectAction(int input, bool state)
        {
            if (state)
                return;
            if (input == KeyConstants.FUNC_D)
            {
                DoCommandAction();
                return;
            } 
            else if (input == KeyConstants.LEFT || input== KeyConstants.RIGHT)
            {
                int action = (int)_currentCommand;
                if (input == KeyConstants.LEFT)
                    action--;
                else if (input == KeyConstants.RIGHT)
                    action++;
                if (action > 3)
                    action = 0;
                else if (action < 0)
                    action = 3;
                _currentCommand = (CommandOptions)action;
            }
            else if(input == KeyConstants.FUNC_COIN_RAD && _currentCommand == CommandOptions.RadX)
            {
                _radXTakeCount++;
                if(_radXTakeCount > 3)
                    _radXTakeCount = 1; 
            }

            switch (_currentCommand)
            {
                case CommandOptions.RadX: WriteInput(_radXTakeCount+" RadX nehmen"); break;
                case CommandOptions.PureLive: WriteInput("PureLive nehmen"); break;
                case CommandOptions.RadAway: WriteInput("RadAway nehmen"); break;
                case CommandOptions.ReadRads: WriteInput("Strahlung messen"); break;
            }
        }

        private void DoCommandAction()
        {
            WriteInput("");

            switch (_currentCommand)
            {
                case CommandOptions.RadX: TakeRadX(_radXTakeCount); break;
                case CommandOptions.PureLive: TakePureLive(); break;
                case CommandOptions.RadAway: TakeRadAway(); break;
                case CommandOptions.ReadRads: ReadRads(); break;
            }
            Thread.Sleep(4000);
            ChangeState_WaitForTakeoff();
        }

        private void ReadRads()
        {
            Write(ReadRadsMessage);
            int rads = 150;
            Thread.Sleep(2000); //TODO Messen
            if (ShowRadsCountAnimation != null)
                Thread.Sleep((int)ShowRadsCountAnimation(rads));

            //bis 100 grün, bis 200 gelb, dann rot
            if(rads <= 100) {
                Write(RadResultMessage_Green);
            }
            else if (rads <= 200)
            {
                Write(RadResultMessage_Yellow);
            }
            else
            {
                Write(RadResultMessage_Red);
            }

        }

        private void TakeRadAway()
        {
            Write(TakeRadXMessage);
            Thread.Sleep(2000);
            ReadRads();
        }

        private void TakePureLive()
        {
            Write(TakeRadXMessage);
            Thread.Sleep(2000);
            ReadRads();
        }

        private void TakeRadX(int _radXTakeCount)
        {
            Write(TakeRadXMessage);
            Thread.Sleep(2000);
            ReadRads();
        }

        public void Dispose()
        {
            _dongleConnector.Close();
        }
    }
}
