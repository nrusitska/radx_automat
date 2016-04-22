using RadBoyBusinessLogic;
using RadBoyBusinessLogic.API.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RadXAutomat.NfcDongle
{
    public class NfcDongleWrapper
    {
        const string WRITE_KEY = null;
        public event EventHandler<string> TagFound;
        public event EventHandler TagLost;
        Thread _worker;
        bool _cancel;

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
        
        CancellationTokenSource _nfcCancelSource = new CancellationTokenSource();
        private void nfcSearchWorker()
        {
            try
            {
                CreateRadApi();
                while (!_cancel)
                {
                    Debug.WriteLine("Begin nfc work loop");
                    lock (_api)
                    {
                        Debug.WriteLine("FindTags");
                        var task = _api.FindTags();
                        var cont = task.ContinueWith(t => { System.Diagnostics.Debug.WriteLine("task1 " + t.Status); });
                        Debug.WriteLine("task.wait");
                        
                        cont.Wait(_nfcCancelSource.Token);
                        
                        cont.Dispose();
                        task.Dispose();
                    }
                    Debug.WriteLine("tag task completed.");
                    Thread.Sleep(500);
                    while (IsTagConnected() && !_cancel)
                    {
                        //Leider wird beim Entfernen gerade das NoTagFound-Event nicht ausgelöst. Momentan als Krücke:
                        //immer wieder _api.GetWrittenMilliRads() aufrufen, bis eine Exception kommt, weil das Tag weg ist...
                        // nicht schön, aber es geht wohl nicht anders...
                        Thread.Sleep(250);
                    }
                    RaiseTagNotFound();
                    Debug.WriteLine("tag disconnected - restarting search.");
                }
                DisposeRadApi();
            }
            catch (Exception ex)
            {
                _cancel = true;
                Debug.WriteLine(ex.ToString());
                RaiseTagNotFound();
            }
        }
        void CreateRadApi()
        {
            if (_api != null)
                DisposeRadApi();
            _api = new RadApi(WRITE_KEY);
            _api.TagFound += Api_TagFound;
            _api.NoTagFound += Api_NoTagFound;
        }
        void DisposeRadApi()
        {
            if (_api == null)
                return;
            _api.TagFound -= Api_TagFound;
            _api.NoTagFound -= Api_NoTagFound;
            try
            {
                _api.Dispose();
            }
            catch { Debug.WriteLine("Error at _api.Dispose()"); }
            _api = null;
        }
        private void Api_NoTagFound()
        {
            Debug.WriteLine("no tags found");
            RaiseTagNotFound();
        }

        private void Api_TagFound(List<DeskIDWrapper.NFC.Tag> list)
        {
            DeskIDWrapper.NFC.Tag foundTag = null;
            foundTag = list.First();
            Debug.WriteLine("tag found: " + foundTag.Id);
            if (TagFound != null && !_cancel)
                TagFound(this, foundTag.Id);
        }

        public void BeginSearch()
        {
            _cancel = false;

            _worker = new Thread(nfcSearchWorker) { Name= "nfcSearchWorker" };
            _worker.Start();           
        }

        public void Close()
        {
            _cancel = true;
            if (_worker.IsAlive)
            {
                _nfcCancelSource.Cancel();
                _worker.Join(200);
                if(_worker.IsAlive)
                    _worker.Abort();
            }
            _worker = null;
            DisposeRadApi();
        }

        public bool IsTagConnected()
        {
            try
            {
                lock (_api)
                {
                    _api.GetWrittenMilliRads();//wirft unterschiedliche Exceptions, wenn das Tag weg ist.
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }

        public int GetRads()
        {
            try
            {
                lock (_api)
                {
#if DEBUG
                    return _api.GetWrittenMilliRads() / 1000;
#else
                    return _api.CalculateCurrentMilliRads() / 1000;
#endif
                }
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
                lock (_api)
                {
                    return _api.GetWrittenMilliRads();
                }
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
                lock (_api)
                {
                    _api.TakeRadEx(count);
                }
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
                lock (_api)
                {
                    _api.TakeRadAway();
                }
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
                lock (_api)
                {
                    _api.TakePureLife();
                }
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
                lock (_api)
                {
                    _api.DoDecon();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                RaiseTagNotFound();
            }
        }

        
    }
}
