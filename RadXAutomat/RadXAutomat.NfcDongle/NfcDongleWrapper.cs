using RadBoyBusinessLogic;
using RadBoyBusinessLogic.API.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace RadXAutomat.NfcDongle
{
    public class NfcDongleWrapper
    {
        const string WRITE_KEY = null;
        public event EventHandler<string> TagFound;
        public event EventHandler TagLost;
        Thread _worker;
        bool _cancel;
        public bool IsTagConnected { get; private set; }
        RadApi _api;
//         public void CancelSearch()
//         {
//             _cancel = true;
//         }
        void RaiseTagNotFound()
        {
            if (TagLost != null && !_cancel)
                TagLost(this, null);
        }
        public void BeginSearch()
        {
            _cancel = false;
            DeskIDWrapper.NFC.Tag foundTag = null;
            _api = new RadApi(WRITE_KEY);
            _api.TagFound += (list) => 
            {
                foundTag = list.First();
                IsTagConnected = true;
                Debug.WriteLine("tag found: " + foundTag.Id);
                if(TagFound != null && !_cancel)
                    TagFound(this, foundTag.Id);
            };
            _api.NoTagFound += () => 
            {
                IsTagConnected = false;
                Debug.WriteLine("no tags found");
                RaiseTagNotFound();
            };
            _worker = new Thread( ()=>
            {
                try
                {
                    while (!_cancel)
                    {
                        var task = _api.FindTags();
                        var cont = task.ContinueWith(t => { System.Diagnostics.Debug.WriteLine("task1 "+t.Status); });
                        cont.Wait();
                        cont.Dispose();
                        task.Dispose();
                        Debug.WriteLine("tag task completed.");
                        Thread.Sleep(500);
                        while (IsTagConnected)
                        {
                            //Leider wird beim Entfernen gerade das NoTagFound-Event nicht ausgelöst. Momentan als Krücke:
                            //immer wieder _api.GetWrittenMilliRads() uafrufen, bis eine Exception kommt, weil das Tag weg ist...
                            // nicht schön, aber es geht wohl nicht anders...
                            try
                            {
                                Thread.Sleep(10);
                                _api.GetWrittenMilliRads();//wirft unterschiedliche Exceptions, wenn das Tag weg ist.
                            }
                            catch(Exception ex)
                            {
                                Debug.WriteLine(ex.ToString());
                                break;
                            }
                        }
                            
                        Debug.WriteLine("tag disconnected - restarting search.");
                    }
                }
                catch (ThreadAbortException){ _cancel = true; }       
            }  );
            _worker.Start();           
        }

        public void Close()
        {
            _cancel = true;
            _worker.Abort();
            _worker = null;
            _api.Dispose();
        }

        public int GetRads()
        {
            try
            {
#if DEBUG
                return _api.GetWrittenMilliRads() / 1000;
#else
                return _api.CalculateCurrentMilliRads() / 1000;
#endif

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                RaiseTagNotFound();
                return -1;
            }


        }
        public int GetMilliRads()
        {
            try
            { 
                return _api.GetWrittenMilliRads();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                RaiseTagNotFound();
                return -1;
            }
        }

        public void TakeRadX(int count)
        {
            try
            {
                _api.TakeRadEx(count);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                RaiseTagNotFound();
            }
        }

        public void TakeRadAway()
        {
            try
            {
                _api.TakeRadAway();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                RaiseTagNotFound();
            }
        }

        public void TakePureLife()
        {
            try
            {
                _api.TakePureLife();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                RaiseTagNotFound();
            }
        }

        public void DoDecon()
        {
            try
            {
                _api.DoDecon();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                RaiseTagNotFound();
            }
        }

        
    }
}
