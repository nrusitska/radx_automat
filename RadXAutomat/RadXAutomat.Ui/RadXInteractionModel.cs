using RadXAutomat.Audio;
using RadXAutomat.Data;
using RadXAutomat.Model;
using RadXAutomat.NfcDongle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;

namespace RadXAutomat.Ui
{
    public delegate void RadXWrite(string text);

    public class RadXInteractionModel : IDisposable
    {
        enum ModelState
        {
            locked, waiting, dongleReadySelectAction, waitingForTakeoff, showStatistics
        }
        enum CommandOptions
        {
            RadX=0,
            PureLive=1,
            RadAway=2,
            ReadRads=3
        }

#if DEBUG
        const bool IS_DEMO = true;
#else
        const bool IS_DEMO = false;
#endif 
        const string FirstMessage = "Hi! Ich bin der R4DB.01 - aber meine Freunde nennen mich RadBoy.\n"
            +"Also wenn ich welche hätte, die noch leben würden.\n"
            +"Oder ich überhaupt je welche gehabt hätte.";
        const string LockMessage = "Na dann beweis mir mal, dass Du hier was zu melden hast!";
        const string WelcomeMessage = "Alles klar Ödländer, dann leg mal Deine Hand den Scanner, und wir schauen uns das mal an...";
        const string DongleFoundMessage = "Ah, da bist Du ja.";
        const string DongleFoundMessage_Jack = "Na das Händchen kenn ich doch! Ladies and Gentleman - Jack Bones is in the house!";

        const string ShowOptions = "Dann lass uns mal loslegen. Wenn Du RadX nehmen willst, kann es gleich losgehen.\n"
            +"Sag mir mit dem RadX-Knopf, wie viele du nehmen willst.\n"
            +"Oder Du schaltest mit dem Joystick links oder rechts durch die Möglichkeiten. Du kannst \n"
            +"[RadX nehmen]\n[PureLive nehmen]\n[RadAway nehmen] oder\n[Strahlung auslesen]\n"
            +"Mit [D] geht's dann los!";
        const string WaitForTakeoffMessage = "So das war's für Dich. Dann mal raus hier und mach' Platz für den nächsten!";

        const string TakeRadXMessage = "Na dann mal rein mit dem guten Zeug!";
        const string ReadRadsMessage = "Dann wollen wir mal sehen, wie verstrahlt Du wirklich bist...\n"
            +"Strahlungsanalyse läuft";
        const string RadResultMessage_Green = "Alle Achtung! Du bist so sauber, wie frisch aus dem Bunker!";
        const string RadResultMessage_Yellow = "Pass mal langsam auf, wo Du Dich so rumtreibst!";
        const string RadResultMessage_Red = "Oh oh. Das sieht böse aus. Vielleicht gehst Du einfach mal wo anders hin. Irgendwo wo Du keine anderen Menschen verstahlen kannst.";
        const string RadResultMessage_Error = "Hoppla, da ist was schief gelaufen. Fangen wir nochmal an.";

        const string StatisticMessage = "Dann lasst uns mal schauen, was wir bisher alles geschafft haben:\n";
        const string StatisticMessage_DeleteOption = "Drücke [A] um die Statistik zu löschen. Zurück geht's mit [B]";
        const string DeleteStatistic_Ask = "Sicher? Dann bestätige mit [C]";
        const string DeleteStatistic_Perform = "Und weg damit.";
        public RadXWrite Write { get; set; }
        public RadXWrite WriteInput { get; set; }
        public Func<int,double> ShowRadsCountAnimation { get; set; }

        private ModelState _state;
        private Action<int, bool> _currentStateHandleKey;
        private CommandOptions _currentCommand;
        private int _radXTakeCount = 0;
        private NfcDongleWrapper _dongleConnector;
        public Dispatcher Dispatcher { get; private set; }
        public RadXInteractionModel()
        {
            var thr = new Thread(new ThreadStart(() =>
            {
                Dispatcher = Dispatcher.CurrentDispatcher;
                Dispatcher.Run();
            }))
            { Name = "RadUI-Interaction-Thread", Priority = ThreadPriority.AboveNormal };
            thr.Start();
            while (thr.ThreadState != System.Threading.ThreadState.Running)
                thr.Join(10);
            _dongleConnector = new NfcDongleWrapper();
            if (!IS_DEMO)
            {
                _dongleConnector.TagFound += _dongleConnector_TagFound;
                _dongleConnector.TagLost += _dongleConnector_TagLost;
                _dongleConnector.BeginSearch();
            }
        }

        private void _dongleConnector_TagLost(object sender, EventArgs e)
        {
            Debug.WriteLine("TagLost, state:" + _state );
            if(!IS_DEMO && _state == ModelState.waitingForTakeoff || _state == ModelState.dongleReadySelectAction)
                ChangeState_Waiting();
        }

        private void _dongleConnector_TagFound(object sender, string e)
        {
            Debug.WriteLine("TagFound, state:" + _state+" id: "+e);
            if (!IS_DEMO && _state == ModelState.waiting)
                ChangeState_ReadyForAction();
        }

        public void Start()
        {
            if (this.Dispatcher == null)
                Thread.Sleep(100);
            this.Dispatcher.BeginInvoke(new Action(() =>
           {
               try
               {
                   if (IS_DEMO)
                       Demo();
                   else
                       ChangeState_Locked();
               }
               catch (System.Exception ex)
               {
                   ;
               }
           }));

            //Demo();
        }

        public void DoInput(Key key, bool keyState)
        {
            Dispatcher.BeginInvoke(new Action(() => {
                if (HandleKey_CheckIfLocked((int)key, keyState))
                {
                    ChangeState_Locked();
                }
                else
                {
                        if (_currentStateHandleKey != null)
                            _currentStateHandleKey((int)key, keyState);
                }
            }));
        }
        void Demo()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                //                 Thread.Sleep(4000);
                //                 ChangeState_Waiting();
                //                 Thread.Sleep(5000);
                //                 ChangeState_ReadyForAction();
                //ReadRads();
                ChangeState_Waiting();
                //ChangeState_ReadyForAction();
            }));          
        }
        bool firstStart = true;
        void ChangeState_Locked()
        {
            _state = ModelState.locked;
            _currentStateHandleKey = HandleKey_LockedWaitForUnlock;
            WriteInput("");
            if(firstStart)
            {
                Write(FirstMessage);
                AudioManager.Instance.PlaySound("First Message_e.mp3");
                Thread.Sleep(4000);
            }
            Write(LockMessage);
            firstStart = false;
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

        bool HandleKey_CheckIfLocked(int input, bool state)
        {
            if (!state
                && (input == KeyConstants.LOCK_1 || input == KeyConstants.LOCK_2))
                return true;
            else
                return false;
        }

        void ChangeState_ShowStatistics()
        {            
            _state =ModelState.showStatistics;
            _currentStateHandleKey = HandleKey_ShowStatistics;
            WriteInput("");
            using (var repo = new MedicationIntakeRepository())
            {
                var rep = repo.GetMedicationReport();
                string reportMessage = string.Format(
                    "RadX:     {0}\n"+
                    "PureLive: {1}\n"+
                    "RadAway:  {2}\n",
                    rep.RadX,rep.PureLive, rep.RadAway);
                Write(StatisticMessage + "\n" + reportMessage + "\n" + StatisticMessage_DeleteOption);
            }


        }

        void ChangeState_Waiting()
        {
            _state = ModelState.waiting;
            if(IS_DEMO)
            {
                _currentStateHandleKey = (k, s) => {
                    if (k == 37 && s) ChangeState_ReadyForAction();
                    else
                        HandleKey_Waiting(k, s);
                };
            }
            else
                _currentStateHandleKey = HandleKey_Waiting;
            WriteInput("");
            Write(WelcomeMessage);
            if (_dongleConnector.IsTagConnected())
            {
                Thread.Sleep(1000);
                if (_dongleConnector.IsTagConnected() && _state == ModelState.waiting)
                {
                    Debug.WriteLine("ChangeState_Waiting -> ChangeState_ReadyForAction");
                    ChangeState_ReadyForAction();
                }
            }
        }
        void HandleKey_Waiting(int input, bool state)
        {
            if (state)
                return;
            else if (input == KeyConstants.FUNC_B)
            {
                ChangeState_ShowStatistics();
            }
        }
        void HandleKey_ShowStatistics(int input, bool state)
        {
            if (state)
                return;

            if (input == KeyConstants.FUNC_A)
            {
                _currentStateHandleKey = (k, s) => 
                {
                    if (!s)
                    {
                        if(k == KeyConstants.FUNC_C)
                        {
                            using(var repo = new MedicationIntakeRepository())
                            {
                                repo.ClearStatistic();
                            }

                        }
                        ChangeState_ShowStatistics();
                    }
                };
                Write(DeleteStatistic_Ask);

            }
            else if (input == KeyConstants.FUNC_B)
            {
                ChangeState_Waiting();
            }           
        }
        void ChangeState_WaitForTakeoff()
        {
            _state = ModelState.waitingForTakeoff;
            if (IS_DEMO)
            {
                _currentStateHandleKey = (k, s) =>
                {
                    if (k == 37 && s)
                        ChangeState_Waiting();                    
                };
            }
            else
                _currentStateHandleKey = null;
            WriteInput("");
            Write(WaitForTakeoffMessage);
        }

        void ChangeState_ReadyForAction()
        {
            Debug.WriteLine("Enter ChangeState_ReadyForAction");
            _radXTakeCount = 2;
            _state = ModelState.dongleReadySelectAction;
            _currentStateHandleKey = HandleKey_SelectAction;
            Write(DongleFoundMessage);
            Thread.Sleep(2000);
            Write(ShowOptions);
            HandleKey_SelectAction(int.MinValue, false);
            Debug.WriteLine("Exit ChangeState_ReadyForAction");
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
            try {
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
            catch (InvalidOperationException)
            {
                ChangeState_Waiting();
            }
        }

        private void ReadRads()
        {
            Write(ReadRadsMessage);
            Thread.Sleep(2000);
            int rads = 0;
            if (IS_DEMO)
            { 
                Thread.Sleep(2000);
                rads = 175;
            }
            else
                rads = _dongleConnector.GetRads();
            if (ShowRadsCountAnimation != null)
                Thread.Sleep((int)ShowRadsCountAnimation(rads));

            //bis 100 grün, bis 200 gelb, dann rot
            if(rads < 0)
            {
                Write(RadResultMessage_Error);
                Thread.Sleep(2000);
                throw new InvalidOperationException();
            }
            else if(rads <= 100)
            {
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
            if (IS_DEMO)
                Thread.Sleep(2000);
            else
                _dongleConnector.TakeRadAway();
            using(var repo = new MedicationIntakeRepository())
            {
                repo.RecordIntake(MedicationType.RadAway);
                repo.CommitOrRollbackOnError();
            }
            ReadRads();
        }

        private void TakePureLive()
        {
            Write(TakeRadXMessage);
            if (IS_DEMO)
                Thread.Sleep(2000);
            else
                _dongleConnector.TakePureLife();
            using (var repo = new MedicationIntakeRepository())
            {
                repo.RecordIntake(MedicationType.PureLive);
                repo.CommitOrRollbackOnError();
            }
            ReadRads();
        }

        private void TakeRadX(int _radXTakeCount)
        {
            Write(TakeRadXMessage);
            if (IS_DEMO)
                Thread.Sleep(2000);
            else
                _dongleConnector.TakeRadX(_radXTakeCount);
            using (var repo = new MedicationIntakeRepository())
            {
                repo.RecordIntake(MedicationType.RadX,_radXTakeCount);
                repo.CommitOrRollbackOnError();
            }
            ReadRads();
        }

        public void Dispose()
        {
            Dispatcher.InvokeShutdown();
            _dongleConnector.Close();
        }
    }
}
