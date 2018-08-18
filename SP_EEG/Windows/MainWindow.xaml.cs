using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;
using SP_EEG.Models;

namespace SP_EEG
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EEGController _eegController;
        //private BrickController _brickController;
        private EventModel_Emotiv _event;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Init()
        {
            _event = new EventModel_Emotiv()
            {
                EngineStatus = "Disconnected",
                BatteryStatus = 0,
                WifiStatus = Emotiv.EdkDll.IEE_SignalStrength_t.NO_SIG,
                SessionStatus = 0f,
                MainScreen = this
            };

            _event.PropertyChanged += Event_PropertyChanged;
            DataContext = _event;
            _eegController = new EEGController(_event);
            _event.InitTrainedActions();
            //brickController = new BrickController("COM3");
        }

        private void Event_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TrainingStatus")
            {
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        prgBar.Visibility = _event.TrainingStatus ? Visibility.Visible : Visibility.Hidden;

                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (e.PropertyName == "ContactQuality")
            {
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        _event.SetContactQuality(this);
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (e.PropertyName == "WifiStatus")
            {
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        _event.SetWifiStatus(this);
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (e.PropertyName == "EngineStatus")
            {
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        _event.SetEngineStatus(this);
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (e.PropertyName == "BatteryStatus")
            {
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        _event.SetBatteryStatus(this);
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }



        /// <summary>
        /// Window loaded event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
            //await brickController.ConnectBrick();
            //await brickController.ChangePolarity();
        }

        private void TrainingStarted()
        {
            prgBar.Visibility = Visibility.Visible;
        }

        private void TrainingFinished()
        {
            prgBar.Visibility = Visibility.Hidden;
        }

        private void connHs_Click(object sender, RoutedEventArgs e)
        {
            _eegController.Start();
        }

        private void trainNeutral_Click(object sender, RoutedEventArgs e)
        {
            _eegController.TrainNeutral();
        }

        private void trainPush_Click(object sender, RoutedEventArgs e)
        {
            _eegController.TrainPush();
        }

        private void trainPull_Click(object sender, RoutedEventArgs e)
        {
            _eegController.TrainPull();
        }

        private void trainLeft_Click(object sender, RoutedEventArgs e)
        {
            _eegController.TrainLeft();
        }

        private void trainRight_Click(object sender, RoutedEventArgs e)
        {
            _eegController.TrainRight();
        }


        private void disHs_Click(object sender, RoutedEventArgs e)
        {
            _eegController.DisconnectEngine();
        }

        /// <summary>
        /// Window closing event, clean up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Disconnect from both Headset and the Robot
            DisconnectDevices();
        }

        private void DisconnectDevices()
        {
            try
            {
                _eegController.DisconnectEngine();
                //_brickController.DisconnectBrick();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
