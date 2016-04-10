using RadBoyBusinessLogic;
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
        Thread _worker;
        bool _cancel;
        public bool IsTagConnected { get; private set; }
        RadApi _api;
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
                if(TagFound != null)
                    TagFound(this, foundTag.Id);
            };
            _api.NoTagFound += () => 
            {
                IsTagConnected = false;
                Debug.WriteLine("no tags found");
                if (TagFound != null)
                    TagFound(this, null);
            };
            _worker = new Thread( ()=>
            {
                try
                {
                    while (!_cancel)
                    {
                        var task = _api.FindTags();
                        var cont = task.ContinueWith(t => { System.Diagnostics.Debug.WriteLine(t.Status); });
                        cont.Wait();
                        Debug.WriteLine("tag task completed.");
                        while (IsTagConnected)
                            Thread.Sleep(50);
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
            return _api.CalculateCurrentMilliRads();
        }
        public int GetMilliRads()
        {
            return _api.GetWrittenMilliRads();
        }

        public void TakeRadX(int count)
        {
            _api.TakeRadEx(count);
        }

        public void TakeRadAway()
        {
            _api.TakeRadAway();
        }

        public void TakePureLife()
        {
            _api.TakePureLife();
        }

        public void DoDecon()
        {
            _api.DoDecon();
        }

        
    }
}
