using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroSky.ThinkGear;
using static SP_EEG.Globals;

namespace SP_EEG
{
    public class EEGController_MindWave
    {
        private Connector _connector;
        private EventModel_MindWave _event;
        public double Attention { get; private set; }
        public double Meditation { get; private set; }

        public double EegPowerDelta { get; private set; }
        public double EegPowerTheta { get; private set; }
        public double EegPowerAlpha1 { get; private set; }
        public double EegPowerAlpha2 { get; private set; }
        public double EegPowerBeta1 { get; private set; }
        public double EegPowerBeta2 { get; private set; }
        public double EegPowerGamma1 { get; private set; }
        public double EegPowerGamma2 { get; private set; }

        public string ComPort { get; set; }

        public void SetEv3Status(string status)
        {
            _event.Ev3Status = status;
        }

        public void SetConnBrick(bool status)
        {
            _event.ConnBrick = status;
        }

        public void SetDissBrick(bool status)
        {
            _event.DissBrick = status;
        }

        public EEGController_MindWave(EventModel_MindWave e)
        {
            _event = e;
            _connector = new Connector();
            SetUpConnector();
            ComPort = Globals.DefaultPort;
        }


        public void SetUpConnector()
        {
            _connector.DeviceConnected += new EventHandler(OnDeviceConnected);
            _connector.DeviceFound += new EventHandler(OnDeviceFound);
            _connector.DeviceNotFound += new EventHandler(OnDeviceNotFound);
            _connector.DeviceConnectFail += new EventHandler(OnDeviceNotFound);
            _connector.DeviceDisconnected += new EventHandler(OnDeviceDisconnected);
            _connector.DeviceValidating += new EventHandler(OnDeviceValidating);
        }

        public void Connect(string port)
        {
            _connector.Connect(port);
            _event.ConnHs = false;
        }

        public void Connect()
        {
            _connector.ConnectScan(ComPort);
            //connector.Find();
            _event.ConnHs = false;
        }

        public void Disconnect()
        {
            _connector.Disconnect();
            _connector.Close();
            _event.DissHs = false;
        }

        private void OnDeviceConnected(object sender, EventArgs e)
        {
            Connector.DeviceEventArgs deviceEventArgs = (Connector.DeviceEventArgs)e;
            Console.WriteLine("Device found on: " + deviceEventArgs.Device.PortName);
            _event.HeadsetStatus = Globals.CONNECTED_STATUS;

            deviceEventArgs.Device.DataReceived += new EventHandler(OnDataReceived);
            _event.ConnHs = false;
            _event.DissHs = true;
        }

        private void OnDeviceValidating(object sender, EventArgs e)
        {
            Console.WriteLine("Validating...");
        }

        private void OnDeviceDisconnected(object sender, EventArgs e)
        {
            Console.WriteLine("Headset disconnected!");
            _event.HeadsetStatus = Globals.CONNECTED_STATUS;
            _event.ConnHs = true;
        }

        private void OnDeviceNotFound(object sender, EventArgs e)
        {
            Console.WriteLine("No devices found.");
            _event.ConnHs = true;
        }

        private void OnDeviceFound(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SetAttention(double val)
        {
            Attention = val;
        }

        private void SetMeditation(double val)
        {
            Meditation = val;
        }

        private void SetEegPowerDelta(double val)
        {
            EegPowerDelta = val;
        }

        private void SetEegPowerTheta(double val)
        {
            EegPowerTheta = val;
        }

        private void SetEegPowerAlpha1(double val)
        {
            EegPowerAlpha1 = val;
        }

        private void SetEegPowerAlpha2(double val)
        {
            EegPowerAlpha2 = val;
        }

        private void SetEegPowerBeta1(double val)
        {
            EegPowerBeta1 = val;
        }

        private void SetEegPowerBeta2(double val)
        {
            EegPowerBeta2 = val;
        }

        private void SetEegPowerGamma1(double val)
        {
            EegPowerGamma1 = val;
        }

        private void SetEegPowerGamma2(double val)
        {
            EegPowerGamma2 = val;
        }



        private void OnDataReceived(object sender, EventArgs e)
        {
            Device d = (Device) sender;
            Device.DataEventArgs de = (Device.DataEventArgs) e;

            TGParser tgParser = new TGParser();
            tgParser.Read(de.DataRowArray);

            for(int i = 0; i < tgParser.ParsedData.Length; i++)
            {
                if (tgParser.ParsedData[i].ContainsKey("Raw"))
                {
                    //Console.WriteLine("Raw Value:" + tgParser.ParsedData[i]["Raw"]);
                }

                if (tgParser.ParsedData[i].ContainsKey("PoorSignal"))
                {
                    //Console.WriteLine("PQ Value:" + tgParser.ParsedData[i]["PoorSignal"]);
                }

                if (tgParser.ParsedData[i].ContainsKey("EegPowerDelta"))
                {
                    SetEegPowerDelta(tgParser.ParsedData[i]["EegPowerDelta"]);
                    Console.WriteLine("Power Delta: {0}", EegPowerDelta);
                    _event.EegPowerDelta = EegPowerDelta;
                }

                if (tgParser.ParsedData[i].ContainsKey("EegPowerTheta"))
                {
                    SetEegPowerTheta(tgParser.ParsedData[i]["EegPowerTheta"]);
                    Console.WriteLine("Power Theta: {0}", EegPowerTheta);
                    _event.EegPowerTheta = EegPowerTheta;
                }

                if (tgParser.ParsedData[i].ContainsKey("EegPowerAlpha1"))
                {
                    SetEegPowerAlpha1(tgParser.ParsedData[i]["EegPowerAlpha1"]);
                    Console.WriteLine("Power Alpha1: {0}", EegPowerAlpha1);
                    _event.EegPowerAlpha1 = EegPowerAlpha1;
                }

                if (tgParser.ParsedData[i].ContainsKey("EegPowerAlpha2"))
                {
                    SetEegPowerAlpha2(tgParser.ParsedData[i]["EegPowerAlpha2"]);
                    Console.WriteLine("Power Alpha2: {0}", EegPowerAlpha2);

                    _event.EegPowerAlpha2 = EegPowerAlpha2;
                }

                if (tgParser.ParsedData[i].ContainsKey("EegPowerBeta1"))
                {
                    SetEegPowerBeta1(tgParser.ParsedData[i]["EegPowerBeta1"]);
                    Console.WriteLine("Power Beta1: {0}", EegPowerBeta1);
                    _event.EegPowerBeta1 = EegPowerBeta1;
                }

                if (tgParser.ParsedData[i].ContainsKey("EegPowerBeta2"))
                {
                    SetEegPowerBeta2(tgParser.ParsedData[i]["EegPowerBeta2"]);
                    Console.WriteLine("Power Beta2: {0}", EegPowerBeta2);
                    _event.EegPowerBeta2 = EegPowerBeta2;
                }

                if (tgParser.ParsedData[i].ContainsKey("EegPowerGamma1"))
                {
                    SetEegPowerGamma1(tgParser.ParsedData[i]["EegPowerGamma1"]);
                    Console.WriteLine("Power Gamma1: {0}", EegPowerGamma1);
                    _event.EegPowerGamma1 = EegPowerGamma1;
                }

                if (tgParser.ParsedData[i].ContainsKey("EegPowerGamma2"))
                {
                    SetEegPowerGamma2(tgParser.ParsedData[i]["EegPowerGamma2"]);
                    Console.WriteLine("Power Gamma2: {0}", EegPowerGamma2);
                    _event.EegPowerGamma2 = EegPowerGamma2;
                }

                if (tgParser.ParsedData[i].ContainsKey("Attention"))
                {
                    SetAttention(tgParser.ParsedData[i]["Attention"]);
                    //Console.WriteLine("Att Value:" + Attention);
                    _event.AttValue = Attention;
                }

                if (tgParser.ParsedData[i].ContainsKey("Meditation"))
                {
                    SetMeditation(tgParser.ParsedData[i]["Meditation"]);
                    //Console.WriteLine("Med Value:" + Meditation);
                    _event.MedValue = Meditation;
                }

            }
        }
    }
}
