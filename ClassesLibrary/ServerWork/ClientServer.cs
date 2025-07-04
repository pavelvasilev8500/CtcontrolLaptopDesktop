using ClassesLibrary.Client;
using ClassesLibrary.SystemControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ClassesLibrary.ServerWork
{
    public class ClientServer
    {
        private static IPEndPoint _remoteEndPoint;
        private static UdpClient _out = new UdpClient(0);
        private static CancellationTokenSource _ctsOut = new CancellationTokenSource();
        private static CancellationTokenSource _ctsIn = new CancellationTokenSource();
        private static readonly string _name = GenerateClientId.Id().Item2;
        private static string _mobileClient = "client #1";
        private static List<string> _avalableClients = new List<string>();
        private SendDataModel _inData = new SendDataModel
        {
            SenderName = string.Empty,
            ReciverName = string.Empty,
            Data = string.Empty,
            Tag = string.Empty
        };

        //Hello World!

        public ClientServer()
        {
            _remoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
            Task.Run(ListenData);
            var connectionConfirm = new Thread(() =>
            {
                while (_inData.Data.ToString() != "Ok")
                {
                    Connect();
                    Debug.WriteLine(_inData.Data);
                    Thread.Sleep(2000);
                }
            });
            connectionConfirm.Name = "ConnectionConfirm";
            connectionConfirm.Start();
            Task.Run(() =>
            {
                while (true)
                {
                    if (_avalableClients.Count > 0)
                    {
                        if (_inData.Tag == "Data")
                            if (_avalableClients.First(c => c == _mobileClient) != "")
                                break;
                    }
                }
                MessageBox.Show("Connect");
                SendTelemetry();
            });
        }

        private async Task TaskAsync()
        {

        }

        private void Connect()
        {
            var connect = new SendDataModel
            {
                SenderName = _name,
                ReciverName = "",
                Data = null,
                Tag = "Connect"
            };
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(connect));
            Send(data, data.Length);
        }

        private async Task SendTelemetry()
        {
            while(!_ctsOut.IsCancellationRequested)
            {
                var sendData = CreateJson.Create("");
                var data = new SendDataModel
                {
                    SenderName = _name,
                    ReciverName = _mobileClient,
                    Data = sendData,
                    Tag = "Data"
                };
                var CtData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
                Send(CtData, CtData.Length);
                await Task.Delay(1000);
            }
        }

        private async Task ListenData()
        {
            while (!_ctsIn.IsCancellationRequested)
            {
                try
                {
                    UdpReceiveResult result = await _out.ReceiveAsync();
                    _inData = JsonConvert.DeserializeObject<SendDataModel>(Encoding.UTF8.GetString(result.Buffer));
                    switch(_inData.Tag)
                    {
                        case "Clients":
                            var avalableClients = JsonConvert.DeserializeObject<List<string>>(_inData.Data.ToString());
                            _avalableClients.AddRange(avalableClients);
                            break;
                        case "Data":
                            switch(_inData.Data.ToString())
                            {
                                case "1":
                                    Disconnect();
                                    SystemControl.halt(false, false);
                                    break;
                                case "2":
                                    Disconnect();
                                    SystemControl.halt(true, false);
                                    break;
                                case "3":
                                    Disconnect();
                                    SystemControl.Sleep(false, false, false);
                                    break;
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                }
                await Task.Delay(1000);
            }
        }

        private static async void Send(byte[] data, int dataLenght)
        {
            try
            {
                await _out.SendAsync(data, dataLenght, _remoteEndPoint);
            }
            catch (Exception)
            {
            }
        }

        private void Disconnect()
        {
            var disconnect = new SendDataModel
            {
                SenderName = _name,
                ReciverName = "",
                Data = null,
                Tag = "Disconnect"
            };
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(disconnect));
            Send(data, data.Length);
        }
    }
}
