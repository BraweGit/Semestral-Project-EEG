using System;
using System.ComponentModel;
using System.Windows.Media;

namespace SP_EEG.Models
{
    public class EventModel_Emotiv : INotifyPropertyChanged
    {
        public MainWindow MainScreen;
        private string _engineStatus;
        private int _batteryStatus;
        private Emotiv.EdkDll.IEE_SignalStrength_t _wifiStatus;
        private float _sessionStatus;
        private bool _trainingStatus;
        private int _cogAction;
        private float _cogPower;
        private string _trainNeutral;
        private string _trainPush;
        private string _trainPull;
        private string _trainLeft;
        private string _trainRight;
        private int _currentTrainedAction;

        private Emotiv.EdkDll.IEE_EEG_ContactQuality_t[] _contactQuality;

        public bool? ShowDialog()
        {
            bool? result = null;
            try
            {
                MainScreen.Dispatcher.Invoke(() =>
                {
                    var trainingDialog = new DialogWindow(Globals.TrainingQuestinText);
                    result = trainingDialog.ShowDialog();
                });
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return result;
        }

        public void InitTrainedActions()
        {
            TrainNeutral = "NOT TRAINED";
            TrainPush = "NOT TRAINED";
            TrainLeft = "NOT TRAINED";
            TrainRight = "NOT TRAINED";
            TrainPull = "NOT TRAINED";
        }

        public void UpdateTrainedActions(bool? isTrained)
        {
            if (!(bool)isTrained || isTrained == null)
                return;

            switch (_currentTrainedAction)
            {
                case 1:
                    TrainNeutral = "TRAINED";
                    break;
                case 2:
                    TrainPush = "TRAINED";
                    break;
                case 4:
                    TrainPull = "TRAINED";
                    break;
                case 32:
                    TrainLeft = "TRAINED";
                    break;
                case 64:
                    TrainRight = "TRAINED";
                    break;
            }
        }

        /// <summary>
        /// MC_NEUTRAL = 1, MC_PUSH = 2, MC_PULL = 4, 
        /// MC_LEFT = 32, MC_RIGHT = 64,
        /// </summary>
        public int CurrentTrainedAction
        {
            get { return _currentTrainedAction; }
            set
            {
                if (value == _currentTrainedAction)
                    return;

                _currentTrainedAction = value;
                //OnPropertyChanged("CurrentTrainedAction");
            }
        }

        public string TrainNeutral
        {
            get { return _trainNeutral; }
            set
            {
                if (value == _trainNeutral)
                    return;

                _trainNeutral = value;
                OnPropertyChanged("TrainNeutral");
            }
        }

        public string TrainPush
        {
            get { return _trainPush; }
            set
            {
                if (value == _trainPush)
                    return;

                _trainPush = value;
                OnPropertyChanged("TrainPush");
            }
        }

        public string TrainPull
        {
            get { return _trainPull; }
            set
            {
                if (value == _trainPull)
                    return;

                _trainPull = value;
                OnPropertyChanged("TrainPull");
            }
        }

        public string TrainLeft
        {
            get { return _trainLeft; }
            set
            {
                if (value == _trainLeft)
                    return;

                _trainLeft = value;
                OnPropertyChanged("TrainLeft");
            }
        }

        public string TrainRight
        {
            get { return _trainRight; }
            set
            {
                if (value == _trainRight)
                    return;

                _trainRight = value;
                OnPropertyChanged("TrainRight");
            }
        }

        public Emotiv.EdkDll.IEE_EEG_ContactQuality_t[] ContactQuality
        {
            get { return _contactQuality; }
            set
            {
                if (value == _contactQuality)
                    return;

                _contactQuality = value;
                OnPropertyChanged("ContactQuality");
            }
        }

        public float CogPower
        {
            get { return _cogPower; }
            set
            {
                if (value == _cogPower)
                    return;

                _cogPower = value;
                OnPropertyChanged("CogPower");
            }
        }

        /// <summary>
        /// MC_NEUTRAL = 1, MC_PUSH = 2, MC_PULL = 4, 
        /// MC_LEFT = 32, MC_RIGHT = 64,
        /// </summary>
        public int CogAction
        {
            get { return _cogAction; }
            set
            {
                if (value == _cogAction)
                    return;

                _cogAction = value;
                OnPropertyChanged("CogAction");
            }
        }

        public bool TrainingStatus
        {
            get { return _trainingStatus; }
            set
            {
                if (value == _trainingStatus)
                    return;

                _trainingStatus = value;
                OnPropertyChanged("TrainingStatus");
            }
        }

        public void SetEngineStatus(MainWindow w)
        {
            if (_engineStatus == "Disconnected")
                w.engineStatusLbl.Foreground = Globals.DarkestRuby;
            else w.engineStatusLbl.Foreground = Globals.GrassGreen;
        }

        public void SetBatteryStatus(MainWindow w)
        {
            if(BatteryStatus == 0)
            {
                w.batteryStr1.Fill = Globals.White;
                w.batteryStr2.Fill = Globals.White;
                w.batteryStr3.Fill = Globals.White;
                w.batteryStr4.Fill = Globals.White;
            }
            else if (BatteryStatus == 1)
            {
                w.batteryStr1.Fill = Globals.GrassGreen;
                w.batteryStr2.Fill = Globals.White;
                w.batteryStr3.Fill = Globals.White;
                w.batteryStr4.Fill = Globals.White;
            }
            else if (BatteryStatus == 2)
            {
                w.batteryStr1.Fill = Globals.GrassGreen;
                w.batteryStr2.Fill = Globals.GrassGreen;
                w.batteryStr3.Fill = Globals.White;
                w.batteryStr4.Fill = Globals.White;
            }
            else if (BatteryStatus == 3)
            {
                w.batteryStr1.Fill = Globals.GrassGreen;
                w.batteryStr2.Fill = Globals.GrassGreen;
                w.batteryStr3.Fill = Globals.GrassGreen;
                w.batteryStr4.Fill = Globals.White;
            }
            else if (BatteryStatus == 4)
            {
                w.batteryStr1.Fill = Globals.GrassGreen;
                w.batteryStr2.Fill = Globals.GrassGreen;
                w.batteryStr3.Fill = Globals.GrassGreen;
                w.batteryStr4.Fill = Globals.GrassGreen;
            }
        }

        public void SetWifiStatus(MainWindow w)
        {
            if (WifiStatus == 0)
            {
                w.wifiStr1.Fill = Globals.White;
                w.wifiStr2.Fill = Globals.White;
                w.wifiStr3.Fill = Globals.White;
                w.wifiStr4.Fill = Globals.White;
                w.wifiStr5.Fill = Globals.White;
            }
            else if ((int)WifiStatus == 1)
            {
                w.wifiStr1.Fill = Globals.GrassGreen;
                w.wifiStr2.Fill = Globals.GrassGreen;
                w.wifiStr3.Fill = Globals.GrassGreen;
                w.wifiStr4.Fill = Globals.White;
                w.wifiStr5.Fill = Globals.White;
            }
            else
            {
                w.wifiStr1.Fill = Globals.GrassGreen;
                w.wifiStr2.Fill = Globals.GrassGreen;
                w.wifiStr3.Fill = Globals.GrassGreen;
                w.wifiStr4.Fill = Globals.GrassGreen;
                w.wifiStr5.Fill = Globals.GrassGreen;
            }
        }

        public string EngineStatus
        {
            get { return _engineStatus; }
            set
            {
                if (value == _engineStatus)
                    return;

                _engineStatus = value;
                OnPropertyChanged("EngineStatus");
            }
        }

        public int BatteryStatus
        {
            get { return _batteryStatus; }
            set
            {
                if (value == _batteryStatus)
                    return;

                _batteryStatus = value;
                OnPropertyChanged("BatteryStatus");
            }
        }

        public Emotiv.EdkDll.IEE_SignalStrength_t WifiStatus
        {
            get { return _wifiStatus; }
            set
            {
                if (value == _wifiStatus)
                    return;

                _wifiStatus = value;
                OnPropertyChanged("WifiStatus");
            }
        }

        public float SessionStatus
        {
            get { return _sessionStatus; }
            set
            {
                if (value == _sessionStatus)
                    return;

                _sessionStatus = value;
                OnPropertyChanged("SessionStatus");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void SetContactQuality(MainWindow w)
        {
            Brush color = Globals.Black;
            for (int i = 0; i < 18; i++)
            {
                switch (_contactQuality[i])
                {
                    case Emotiv.EdkDll.IEE_EEG_ContactQuality_t.IEEG_CQ_NO_SIGNAL:
                        UpdateQualityStatus(i, color, w);
                        break;

                    case Emotiv.EdkDll.IEE_EEG_ContactQuality_t.IEEG_CQ_VERY_BAD:
                        color = Globals.Red;
                        UpdateQualityStatus(i, color, w);
                        break;

                    case Emotiv.EdkDll.IEE_EEG_ContactQuality_t.IEEG_CQ_POOR:
                        color = Globals.DarkYellow;
                        UpdateQualityStatus(i, color, w);
                        break;

                    case Emotiv.EdkDll.IEE_EEG_ContactQuality_t.IEEG_CQ_FAIR:
                        color = Globals.BrightYellow;
                        UpdateQualityStatus(i, color, w);
                        break;

                    case Emotiv.EdkDll.IEE_EEG_ContactQuality_t.IEEG_CQ_GOOD:
                        color = Globals.BrightGreen;
                        UpdateQualityStatus(i, color, w);
                        break;

                }
            }
        }

        private void UpdateQualityStatus(int i, Brush color, MainWindow w)
        {
            switch (i)
            {
                case 0:
                    w.cms.Fill = color;
                    break;
                case 1:
                    w.drl.Fill = color;
                    break;
                case 3:
                    w.af3.Fill = color;
                    break;
                case 4:
                    w.f7.Fill = color;
                    break;
                case 5:
                    w.f3.Fill = color;
                    break;
                case 6:
                    w.fc5.Fill = color;
                    break;
                case 7:
                    w.t7.Fill = color;
                    break;
                case 8:
                    w.p7.Fill = color;
                    break;
                case 9:
                    w.o1.Fill = color;
                    break;
                case 10:
                    w.o2.Fill = color;
                    break;
                case 11:
                    w.p8.Fill = color;
                    break;
                case 12:
                    w.t8.Fill = color;
                    break;
                case 13:
                    w.fc6.Fill = color;
                    break;
                case 14:
                    w.f4.Fill = color;
                    break;
                case 15:
                    w.f8.Fill = color;
                    break;
                case 16:
                    w.af4.Fill = color;
                    break;

            }
        }
    }
}
