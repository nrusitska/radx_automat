using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace RadXAutomat.SerialButtonConnection
{
    public delegate void ButtonStateHandler(int button, bool state);
    public class SerialButtonConnector
    {
        public event ButtonStateHandler ButtonChanged;
        SerialPort _serial;
        bool _run = true;
        public SerialButtonConnector(string portNameHint)
        {
            _serial = GetSerialPort(115200, portNameHint);
        }

        SerialPort GetSerialPort(int baudRate, string portHint = "")
        {
            if (!string.IsNullOrEmpty(portHint))
                try
                {
                    return new SerialPort(portHint, baudRate);
                }
                catch
                {
                    //round one: wait and try again

                    try
                    {
                        Thread.Sleep(50);
                        return new SerialPort(portHint, baudRate);
                    }
                    catch //round two failed? desired port wont work
                    {; }
                }

            var ports = SerialPort.GetPortNames().ToList();
            ports.Remove(portHint);

            foreach (string port in ports)
            {
                try
                {
                    return new SerialPort(port, baudRate);
                }
                catch {; }
            }
            //if we get here, there was no valid port
            throw new System.IO.IOException("no accessible port found");
        }
        public void Stop()
        {
            _run = false;
            _serial.Close();
            _serial.Dispose();
        }
        public void BeginListening()
        {
            Console.CancelKeyPress += (s, e) => { Stop(); e.Cancel = true; };
            _serial.Open();
            string linePattern = (@"Button:(?'button'\d+) is: (?'value'\d+)");


            while (_run)
            {
                string line = null;
                try
                {
                    line = _serial.ReadLine();
                    line=line.Replace("\r", "");
                }
                catch(Exception ex)
                {
                    Console.Out.WriteLine(ex.ToString());
                    break;
                }
                try
                {
                    var groups = Regex.Match(line, linePattern).Groups;
                    int button = int.Parse(groups["button"].Value);
                    bool value = int.Parse(groups["value"].Value) == 1;
                    if (ButtonChanged != null)
                        ButtonChanged(button, value);
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine(ex.ToString());
                }
            }
            _serial.Close();
        }
    }
}
