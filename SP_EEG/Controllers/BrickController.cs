using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Lego.Ev3.Core;
using Lego.Ev3.Desktop;

namespace SP_EEG
{
    public class BrickController
    {
        private Brick _brick;
        private readonly MainWindow_MinWave _mainWindow;
        private readonly EEGController_MindWave _eegControllerMindWave;
        private readonly EventModel_MindWave _event;
        private string _port;



        public BrickController() { }

        public BrickController(MainWindow_MinWave w, EEGController_MindWave ec, EventModel_MindWave ev)
        {
            _mainWindow = w;
            _eegControllerMindWave = ec;
            _event = ev;

        }

        public void InitCommunication()
        {
            _brick = new Brick(new BluetoothCommunication(Port));
            // Adding event
            _brick.BrickChanged += Brick_BrickChanged;
        }

        public string Port
        {
            get { return _port == null ? Globals.DefaultPort : _port; }
            set
            {
                if (value == _port)
                    return;

                _port = value;
            }
        }

        public async Task ConnectBrick()
        {
            try
            {
                await _brick.ConnectAsync();
                // Play tone when connected
                await _brick.DirectCommand.PlayToneAsync(10, 1000, 300);
                _eegControllerMindWave.SetEv3Status(Globals.CONNECTED_STATUS);
                _eegControllerMindWave.SetConnBrick(false);
                _eegControllerMindWave.SetDissBrick(true);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while connecting to EV3. Included message: {0}", e.Message);
            }

        }

        public void DisconnectBrick()
        {
            try
            {
                _brick.Disconnect();
                _eegControllerMindWave.SetEv3Status(Globals.DISCONNECTED_STATUS);
                _eegControllerMindWave.SetConnBrick(true);
                _eegControllerMindWave.SetDissBrick(false);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while disconnecting from EV3. Included message: {0}", e.Message);
            }

        }

        public async Task ChangePolarity()
        {
            try
            {
                // Change polarity if necessary
                await _brick.DirectCommand.SetMotorPolarityAsync(OutputPort.B | OutputPort.C, Polarity.Backward);
                // After changing the polarity, motors might keep running, make sure to stop them
                await _brick.DirectCommand.StopMotorAsync(OutputPort.All, false);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while changing polarity of EV3. Included message: {0}", e.Message);
            }

        }

        /// <summary>
        /// Fires when new data comes from the lego _brick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Brick_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            //Console.WriteLine("Brick changed!");
            //Console.WriteLine(GetSensorValue());
            if (_mainWindow.chckFollow.IsChecked.HasValue && _mainWindow.chckFollow.IsChecked.Value)
            {
                try
                {
                    //_eegControllerMindWave.Attention
                    await FollowBlackLine(GetSensorValue(), _eegControllerMindWave.Attention);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while following line. Included message: {0}", ex.Message);
                }

            }

        }

        public async Task MoveForward(int? power = null)
        {
            var pow = power == null ? Globals.FORWARD : (int)power;
            //await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.B | OutputPort.C, Globals.FORWARD, Globals.TIME, false);
            try
            {
                await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.B | OutputPort.C, pow);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while moving forward. Included message: {0}", e.Message);
            }

        }

        public async Task MoveForwardForTime()
        {
            try
            {
                await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.B | OutputPort.C, Globals.FORWARD, Globals.TIME, false);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while moving forward. Included message: {0}", e.Message);
            }

        }

        public async Task MoveBackwards()
        {
            try
            {
                await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.B | OutputPort.C, Globals.BACKWARDS);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while moving forward. Included message: {0}", e.Message);
            }

        }

        public async Task MoveBackwardsForTime()
        {
            try
            {
                await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.B | OutputPort.C, Globals.BACKWARDS, Globals.TIME, false);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while moving forward. Included message: {0}", e.Message);
            }

        }

        public async Task MoveLeft(string turn)
        {
            try
            {
                if (turn == Globals.EASY)
                {
                    await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.C, -13);
                    await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.B, 13);
                }
                else if (turn == Globals.SHARP)
                {
                    await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.C, -20);
                    await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.B, 20);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while moving left. Included message: {0}", e.Message);
            }


            // Queue up both commands
            //_brick.BatchCommand.TurnMotorAtPower(OutputPort.C, Globals.BACKWARDS);
            //_brick.BatchCommand.TurnMotorAtPower(OutputPort.B, Globals.FORWARD);
            //// Send them to robot to execute
            //await _brick.BatchCommand.SendCommandAsync();
        }

        public async Task MoveLeftForTime()
        {
            try
            {
                // Queue up both commands
                _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.C, Globals.BACKWARDS, Globals.TIME, false);
                _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.B, Globals.FORWARD, Globals.TIME, false);
                // Send them to robot to execute
                await _brick.BatchCommand.SendCommandAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while moving left for time. Included message: {0}", e.Message);
            }

        }

        public async Task MoveRight(string turn)
        {
            try
            {
                if (turn == Globals.EASY)
                {
                    await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.B, -13);
                    await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.C, 13);
                }
                else if (turn == Globals.SHARP)
                {
                    await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.B, -20);
                    await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.C, 20);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while moving right. Included message: {0}", e.Message);
            }


            //// Queue up both commands
            //_brick.BatchCommand.TurnMotorAtPower(OutputPort.B, Globals.BACKWARDS);
            //_brick.BatchCommand.TurnMotorAtPower(OutputPort.C, Globals.FORWARD);
            //// Send them to robot to execute
            //await _brick.BatchCommand.SendCommandAsync();
        }

        public async Task MoveRightForTime()
        {
            try
            {
                // Queue up both commands
                _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.B, Globals.BACKWARDS, Globals.TIMELONG, false);
                _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.C, Globals.FORWARD, Globals.TIMELONG, false);
                // Send them to robot to execute
                await _brick.BatchCommand.SendCommandAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while moving right for time. Included message: {0}", e.Message);
            }

        }

        public void SubscribeToEvent()
        {
            _brick.BrickChanged += Brick_BrickChanged;
        }

        public async Task MoveSteering(int steering, int power)
        {
            var turn = (int)(power - (Math.Abs(steering) / 100.0) * (2 * power));
            if (steering > 0 && steering <= 100)
            {
                await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.C, power);
                await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.B, turn);
            }
            else if (steering == 0)
            {
                await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.C, power);
                await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.B, power);
            }
            else if (steering >= -100 && steering < 0)
            {
                await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.C, turn);
                await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.B, power);
            }
            else
            {
                Console.WriteLine("Wrong input!");
            }
        }

        public async Task FollowBlackLine(double[] sensorValues, double power)
        {
            #region comments
            //Test(sensorValues, power);
            //return;

            //var midPoint = (73 - 6) / 2 + 6;

            //if (midPoint > sensorValue)
            //    await MoveLeft();
            //else
            //    await MoveRight();

            // ver. 2
            //var bPwr = (73 - sensorValue) * 1;
            //var cPwr = (sensorValue - 6) * 1;

            //await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.C, (int)cPwr);
            //await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.B, (int)bPwr);


            ;
            //var midPoint = (66 - 5) / 2 + 5;
            // 67-69 White Colour, will try 60-75
            //var threshold = 63;
            // Turn coefficient, how sharp steering will be
            // user should try differenct values, recommended is between 0.5 - 0.7
            //var tc = 0.7;
            #endregion

            //int turn;
            //int scaledPower = (int)(power * Globals.TURN_POWER_SCALE);
            //int halfColorThreshold = Globals.COLOR_THRESHOLD/2;

            int turn;
            int scaledPower = (int)(_event.MaxAttentionValue * _event.TurnPowerScaleValue);
            int halfColorThreshold = _event.ColorTresholdValue / 2;

            _event.MaxAttentionValue = (int)power;



            // Experimental
            //if (sensorValues[0] < halfColorThreshold && sensorValues[1] < halfColorThreshold)
            //{
            //    await Stop();
            //    return;
            //}

            //Right sensor sensorValues[0]
            //Left sensor sensorValues[1]
            if (sensorValues[0] > halfColorThreshold && sensorValues[1] < halfColorThreshold)
            {
                turn = (int)(Math.Abs(((sensorValues[1] + sensorValues[0]) / 2) - _event.ColorTresholdValue) * _event.TurnCoefficientValue);
                Console.WriteLine("[{0}],[{1}] MoveRight, Turn: {2}; Power: {3}", sensorValues[0], sensorValues[1], turn, _event.MaxAttentionValue);
                await MoveSteering(turn, scaledPower);
            }
            else
            {
                //Left sensor
                if (sensorValues[1] > halfColorThreshold && sensorValues[0] < halfColorThreshold)
                {
                    turn = (int)((((sensorValues[1] + sensorValues[0]) / 2) - _event.ColorTresholdValue) * _event.TurnCoefficientValue);
                    Console.WriteLine("[{0}],[{1}] MoveLeft, Turn: {2}; Power: {3}", sensorValues[0], sensorValues[1], turn, _event.MaxAttentionValue);
                    await MoveSteering(turn, scaledPower);
                }
                else
                {
                    Console.WriteLine("[{0}],[{1}] MoveForward, Power: {2}", sensorValues[0], sensorValues[1], _event.MaxAttentionValue);
                    await MoveForward(scaledPower);
                }
            }

        }

        public async Task Stop()
        {
            try
            {
                await _brick.DirectCommand.StopMotorAsync(OutputPort.B | OutputPort.C, false);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to stop. Included message: {0}", e.Message);
            }

        }

        public double[] GetSensorValue()
        {
            if (_brick == null) return new double[] { -1, -1 };
            var result = new double[] { _brick.Ports[InputPort.One].SIValue, _brick.Ports[InputPort.Two].SIValue };
            Console.WriteLine("Sensor 1 value is: {0}, sensor 2 value is: {1}", result[0], result[1]);
            return result;
        }

        public async Task ExecuteProgram()
        {
            await _brick.DirectCommand.StartProgramAsync("");
        }
    }
}
