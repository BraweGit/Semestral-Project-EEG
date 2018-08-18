using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Lego.Ev3.Core;
using Lego.Ev3.Desktop;
using System.IO.Ports;
using SP_EEG.Utils;

namespace SP_EEG
{
    /// <summary>
    /// Interaction logic for MainWindow_MinWave.xaml
    /// </summary>
    public partial class MainWindow_MinWave : Window
    {
        private EEGController_MindWave _eegController;
        private BrickController _brickController;
        private EventModel_MindWave _event;
        private SettingsManager _settingsManager;


        public MainWindow_MinWave()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            _event = new EventModel_MindWave()
            {
                MainScreen = this
            };

            _settingsManager = new SettingsManager();
            _event.PropertyChanged += Event_PropertyChanged;
            DataContext = _event;
            _eegController = new EEGController_MindWave(_event);
            _brickController = new BrickController(this, _eegController, _event);
            _event.ComPorts = SerialPort.GetPortNames().ToList();
            this.KeyDown += new KeyEventHandler(MainWindow_MindWave_KeyDown);

            // set-up buttons
            _event.ConnHs = true;
            _event.DissHs = false;
            _event.ConnBrick = true;
            _event.DissBrick = false;

            _settingsManager.ReadSettings(_event);


        }

        async void MainWindow_MindWave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                await _brickController.MoveForward();
            }

            if (e.Key == Key.Down)
            {
                await _brickController.MoveBackwards();
            }

            if (e.Key == Key.Left)
            {
                await _brickController.MoveLeft(Globals.EASY);
            }

            if (e.Key == Key.Right)
            {
                await _brickController.MoveRight(Globals.EASY);
            }

            if (e.Key == Key.RightShift)
            {
                await _brickController.Stop();
            }
        }

        private void Event_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // if I am unable to bind smth
            // can set it up here
        }

        private void connHs_Click(object sender, RoutedEventArgs e)
        {
            _eegController.Connect();
        }

        private void dissHs_Click(object sender, RoutedEventArgs e)
        {
            _eegController.Disconnect();
        }

        private async void connBrick_Click(object sender, RoutedEventArgs e)
        {
            _brickController.InitCommunication();
            await _brickController.ConnectBrick();
            await _brickController.ChangePolarity();
        }

        private void dissBrick_Click(object sender, RoutedEventArgs e)
        {
            _brickController.DisconnectBrick();
        }

        private async void btnForward_Click(object sender, RoutedEventArgs e)
        {
            await _brickController.MoveForward();
        }

        private async void btnBackwards_Click(object sender, RoutedEventArgs e)
        {
            await _brickController.MoveBackwards();
        }

        private async void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            await _brickController.MoveLeft(Globals.EASY);
        }

        private async void btnRight_Click(object sender, RoutedEventArgs e)
        {
            await _brickController.MoveRightForTime();
        }

        private async void btnStop_Click(object sender, RoutedEventArgs e)
        {
            await _brickController.Stop();
        }

        private void btnBlack_Click(object sender, RoutedEventArgs e)
        {
            _event.LineColor = _brickController.GetSensorValue()[0];
        }

        private void btnWhite_Click(object sender, RoutedEventArgs e)
        {
            _event.BorderColor = _brickController.GetSensorValue()[1];
        }

        private async void chckFollow_Checked(object sender, RoutedEventArgs e)
        {
            if (chckFollow.IsChecked.HasValue && !chckFollow.IsChecked.Value)
            {
                await _brickController.Stop();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _eegController.Disconnect();
            _settingsManager.UpdateSettings(_event);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {

        }

        private void comboHsPorts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _eegController.ComPort = (string)comboHsPorts.SelectedItem;
        }

        private void comboEv3Ports_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _brickController.Port = (string)comboEv3Ports.SelectedItem;
        }
    }
}
