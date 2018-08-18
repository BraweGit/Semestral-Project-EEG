using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_EEG
{
    public class EventModel_MindWave : INotifyPropertyChanged
    {
        public MainWindow_MinWave MainScreen;
        private double _attValue;
        private double _medValue;
        private double _lineColor;
        private double _borderColor;

        private double _eegPowerDelta;
        private double _eegPowerTheta;
        private double _eegPowerAlpha1;
        private double _eegPowerAlpha2;
        private double _eegPowerBeta1;
        private double _eegPowerBeta2;
        private double _eegPowerGamma1;
        private double _eegPowerGamma2;

        private string _headsetStatus;
        private string _ev3Status;

        private List<string> _comPorts;

        private bool _connHs;
        private bool _dissHs;

        private bool _connBrick;
        private bool _dissBrick;

        private int _colorTresholdValue;
        private double _turnCoefficientValue;
        private double _forwardPowerScaleValue;
        private double _turnPowerScaleValue;
        private int _maxAttentionValue;

        public int MaxAttentionValue
        {
            get { return _maxAttentionValue; }
            set
            {
                if (value == _maxAttentionValue)
                    return;


                _maxAttentionValue = value < Globals.MIN_ATTENTION ? Globals.MIN_ATTENTION : value;
                _maxAttentionValue = value > Globals.MAX_ATTENTION ? Globals.MAX_ATTENTION : value;
                OnPropertyChanged("MaxAttentionValue");
            }
        }

        public double TurnPowerScaleValue
        {
            get { return _turnPowerScaleValue; }
            set
            {
                if (value == _turnPowerScaleValue)
                    return;

                _turnPowerScaleValue = value < Globals.MIN_TURN_POWER_SCALE ? Globals.MIN_TURN_POWER_SCALE : value;
                _turnPowerScaleValue = value > Globals.MAX_TURN_POWER_SCALE ? Globals.MAX_TURN_POWER_SCALE : value;
                OnPropertyChanged("TurnPowerScaleValue");
            }
        }


        public double ForwardPowerScaleValue
        {
            get { return _forwardPowerScaleValue; }
            set
            {
                if (value == _forwardPowerScaleValue)
                    return;

                _forwardPowerScaleValue = value < Globals.MIN_FORWARD_POWER_SCALE ? Globals.MIN_FORWARD_POWER_SCALE : value;
                _forwardPowerScaleValue = value > Globals.MAX_FORWARD_POWER_SCALE ? Globals.MAX_FORWARD_POWER_SCALE : value;
                OnPropertyChanged("ForwardPowerScaleValue");
            }
        }

        public int ColorTresholdValue
        {
            get { return _colorTresholdValue; }
            set
            {
                if (value == _colorTresholdValue)
                    return;

                _colorTresholdValue = value < Globals.MIN_COLOR_TRESHOLD ? Globals.MIN_COLOR_TRESHOLD : value;
                _colorTresholdValue = value > Globals.MAX_COLOR_TRESHOLD ? Globals.MAX_COLOR_TRESHOLD : value;
                OnPropertyChanged("ColorTresholdValue");
            }
        }

        public double TurnCoefficientValue
        {
            get { return _turnCoefficientValue; }
            set
            {
                if (value == _turnCoefficientValue)
                    return;

                _turnCoefficientValue = value < Globals.MIN_TURN_COEFFICIENT ? Globals.MIN_TURN_COEFFICIENT : value;
                _turnCoefficientValue = value > Globals.MAX_TURN_COEFFICIENT ? Globals.MAX_TURN_COEFFICIENT : value;
                OnPropertyChanged("TurnCoefficientValue");
            }
        }


        public bool ConnBrick
        {
            get { return _connBrick; }
            set
            {
                if (value == _connBrick)
                    return;

                _connBrick = value;
                OnPropertyChanged("ConnBrick");
            }
        }

        public bool DissBrick
        {
            get { return _dissBrick; }
            set
            {
                if (value == _dissBrick)
                    return;

                _dissBrick = value;
                OnPropertyChanged("DissBrick");
            }
        }

        public bool DissHs
        {
            get { return _dissHs; }
            set
            {
                if (value == _dissHs)
                    return;

                _dissHs = value;
                OnPropertyChanged("DissHs");
            }
        }

        public bool ConnHs
        {
            get { return _connHs; }
            set
            {
                if (value == _connHs)
                    return;

                _connHs = value;
                OnPropertyChanged("ConnHs");
            }
        }

        

        public List<string> ComPorts
        {
            get { return _comPorts; }
            set
            {
                if (value == _comPorts)
                    return;

                _comPorts = value;
                OnPropertyChanged("ComPorts");
            }
        }

        public string Ev3Status
        {
            get { return _ev3Status; }
            set
            {
                if (value == _ev3Status)
                    return;

                _ev3Status = value;
                OnPropertyChanged("Ev3Status");
            }
        }

        public string HeadsetStatus
        {
            get { return _headsetStatus; }
            set
            {
                if (value == _headsetStatus)
                    return;

                _headsetStatus = value;
                OnPropertyChanged("HeadsetStatus");
            }
        }

        public double EegPowerDelta
        {
            get { return _eegPowerDelta; }
            set
            {
                if (value == _eegPowerDelta)
                    return;

                _eegPowerDelta = value;
                OnPropertyChanged("EegPowerDelta");
            }
        }

        public double EegPowerTheta
        {
            get { return _eegPowerTheta; }
            set
            {
                if (value == _eegPowerTheta)
                    return;

                _eegPowerTheta = value;
                OnPropertyChanged("EegPowerTheta");
            }
        }

        public double EegPowerAlpha1
        {
            get { return _eegPowerAlpha1; }
            set
            {
                if (value == _eegPowerAlpha1)
                    return;

                _eegPowerAlpha1 = value;
                OnPropertyChanged("EegPowerAlpha1");
            }
        }

        public double EegPowerAlpha2
        {
            get { return _eegPowerAlpha2; }
            set
            {
                if (value == _eegPowerAlpha2)
                    return;

                _eegPowerAlpha2 = value;
                OnPropertyChanged("EegPowerAlpha2");
            }
        }


        public double EegPowerBeta1
        {
            get { return _eegPowerBeta1; }
            set
            {
                if (value == _eegPowerBeta1)
                    return;

                _eegPowerBeta1 = value;
                OnPropertyChanged("EegPowerBeta1");
            }
        }

        public double EegPowerBeta2
        {
            get { return _eegPowerBeta2; }
            set
            {
                if (value == _eegPowerBeta2)
                    return;

                _eegPowerBeta2 = value;
                OnPropertyChanged("EegPowerBeta2");
            }
        }

        public double EegPowerGamma1
        {
            get { return _eegPowerGamma1; }
            set
            {
                if (value == _eegPowerGamma1)
                    return;

                _eegPowerGamma1 = value;
                OnPropertyChanged("EegPowerGamma1");
            }
        }

        public double EegPowerGamma2
        {
            get { return _eegPowerGamma2; }
            set
            {
                if (value == _eegPowerGamma2)
                    return;

                _eegPowerGamma2 = value;
                OnPropertyChanged("EegPowerGamma2");
            }
        }

        public double LineColor
        {
            get { return _lineColor; }
            set
            {
                if (value == _lineColor)
                    return;

                _lineColor = value;
                OnPropertyChanged("LineColor");
            }
        }

        public double BorderColor
        {
            get { return _borderColor; }
            set
            {
                if (value == _borderColor)
                    return;

                _borderColor = value;
                OnPropertyChanged("BorderColor");
            }
        }



        public double AttValue
        {
            get { return _attValue; }
            set
            {
                if (value == _attValue)
                    return;

                _attValue = value;
                OnPropertyChanged("AttValue");
            }
        }

        public double MedValue
        {
            get { return _medValue; }
            set
            {
                if (value == _medValue)
                    return;

                _medValue = value;
                OnPropertyChanged("MedValue");
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
    }
}
