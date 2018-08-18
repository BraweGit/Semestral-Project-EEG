using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emotiv;
using System.Diagnostics;
using System.Threading;
using SP_EEG.Models;

namespace SP_EEG
{
    public class EEGController
    {
        private EmoEngine _engine;
        private bool _diconnect;
        private int _batterChargeLevel;
        private int _maxBatterChargeLevel;

        public Thread EEGThread { get; set; }
        public bool IsRunning { get; set; }

        private EventModel_Emotiv _eventModel;

        public EEGController(EventModel_Emotiv eventModel)
        {
            // Hook up all necessary events for the headset
            SetUpEngine();
            //ConnectEngine();
            _eventModel = eventModel;
        }

        public void Start()
        {
            try
            {
                EEGThread = new Thread(new ThreadStart(Run));
                EEGThread.IsBackground = true;
                EEGThread.Start();
                IsRunning = true;
                ConnectEngine();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                
            }
        }

        public void Run()
        {
            while (IsRunning)
            {
                try
                {
                    _engine.ProcessEvents(1000);
                    //Console.WriteLine("Run Thread!");
                }
                catch (EmoEngineException e)
                {
                    Debug.WriteLine("{0}", e.ToString());
                }
                catch (Exception e)
                {
                    Debug.WriteLine("{0}", e.ToString());
                }
            }

            if (_diconnect)
                _engine.Disconnect();
        }

        public void ConnectEngine()
        {
            // Connecting to the EmoEngine
            try
            {
                _engine.Connect();
                _diconnect = false;
                SetActiveActions();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void DisconnectEngine()
        {
            // Disconnecting from the EmoEngine
            IsRunning = false;
            _diconnect = true;
        }

        private void SetActiveActions()
        {
            EmoEngine.Instance.MentalCommandSetActiveActions(0, (uint)EdkDll.IEE_MentalCommandAction_t.MC_LEFT);
            EmoEngine.Instance.MentalCommandSetActiveActions(0, (uint)EdkDll.IEE_MentalCommandAction_t.MC_RIGHT);
            EmoEngine.Instance.MentalCommandSetActiveActions(0, (uint)EdkDll.IEE_MentalCommandAction_t.MC_PUSH);
            EmoEngine.Instance.MentalCommandSetActiveActions(0, (uint)EdkDll.IEE_MentalCommandAction_t.MC_PULL);
        }

        /// <summary>
        /// Connects all necessary events to the engine
        /// </summary>
        private void SetUpEngine()
        {
            _engine = EmoEngine.Instance;
            _engine.EmoEngineConnected += new EmoEngine.EmoEngineConnectedEventHandler(engine_EmoEngineConnected);
            _engine.EmoEngineDisconnected += new EmoEngine.EmoEngineDisconnectedEventHandler(engine_EmoEngineDisconnected);
            _engine.UserAdded += new EmoEngine.UserAddedEventHandler(engine_UserAdded);
            _engine.UserRemoved += new EmoEngine.UserRemovedEventHandler(engine_UserRemoved);
            _engine.EmoStateUpdated += new EmoEngine.EmoStateUpdatedEventHandler(engine_EmoStateUpdated);
            _engine.EmoEngineEmoStateUpdated += new EmoEngine.EmoEngineEmoStateUpdatedEventHandler(engine_EmoEngineEmoStateUpdated);
            _engine.MentalCommandEmoStateUpdated += new EmoEngine.MentalCommandEmoStateUpdatedEventHandler(engine_MentalCommandEmoStateUpdated);
            _engine.MentalCommandTrainingStarted += new EmoEngine.MentalCommandTrainingStartedEventEventHandler(engine_MentalCommandTrainingStarted);
            _engine.MentalCommandTrainingSucceeded += new EmoEngine.MentalCommandTrainingSucceededEventHandler(engine_MentalCommandTrainingSucceeded);
            _engine.MentalCommandTrainingCompleted += new EmoEngine.MentalCommandTrainingCompletedEventHandler(engine_MentalCommandTrainingCompleted);
            _engine.MentalCommandTrainingRejected += new EmoEngine.MentalCommandTrainingRejectedEventHandler(engine_MentalCommandTrainingRejected);
        }

        #region EEG Headset events
        public void engine_EmoEngineConnected(object sender, EmoEngineEventArgs e)
        {
            Console.WriteLine("Emoengine connected");
            _eventModel.EngineStatus = "Connected";
        }

        public void engine_EmoEngineDisconnected(object sender, EmoEngineEventArgs e)
        {
            Console.WriteLine("Emoengine disconnected");
            _eventModel.EngineStatus = "Disconnected";
        }

        public void engine_UserAdded(object sender, EmoEngineEventArgs e)
        {
            Console.WriteLine("user added ({0})", e.userId);
        }
        public void engine_UserRemoved(object sender, EmoEngineEventArgs e)
        {
            Console.WriteLine("user removed");
        }

        public void engine_EmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
        {
            EmoState es = e.emoState;
            Console.WriteLine("Emostate updated: {0}", e.emoState);
            es.GetBatteryChargeLevel(out _batterChargeLevel, out _maxBatterChargeLevel);
            _eventModel.BatteryStatus = _batterChargeLevel;
            _eventModel.WifiStatus = es.GetWirelessSignalStatus();
            _eventModel.SessionStatus = es.GetTimeFromStart();
            _eventModel.ContactQuality = es.GetContactQualityFromAllChannels();
        }

        public void engine_EmoEngineEmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
        {
            EmoState es = e.emoState;

            Single timeFromStart = es.GetTimeFromStart();

            Int32 headsetOn = es.GetHeadsetOn();

            EdkDll.IEE_SignalStrength_t signalStrength = es.GetWirelessSignalStatus();
            Int32 chargeLevel = 0;
            Int32 maxChargeLevel = 0;
            es.GetBatteryChargeLevel(out chargeLevel, out maxChargeLevel);
        }

        public void engine_MentalCommandEmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
        {
            EmoState es = e.emoState;

            Single timeFromStart = es.GetTimeFromStart();

            EdkDll.IEE_MentalCommandAction_t cogAction = es.MentalCommandGetCurrentAction();
            Single power = es.MentalCommandGetCurrentActionPower();
            Boolean isActive = es.MentalCommandIsActive();
            _eventModel.CogAction = (int)cogAction;
            _eventModel.CogPower = power;

            Console.WriteLine("{0},{1},{2},{3}", timeFromStart, cogAction, power, isActive);
        }

        public void engine_MentalCommandTrainingStarted(object sender, EmoEngineEventArgs e)
        {
            _eventModel.TrainingStatus = true;
            Console.WriteLine("Start MentalCommand Training");        
        }

        public void engine_MentalCommandTrainingSucceeded(object sender, EmoEngineEventArgs e)
        {
            Console.WriteLine("MentalCommand Training Success.");
            //ConsoleKeyInfo cki = Console.ReadKey(true);
            //if (cki.Key == ConsoleKey.A)
            //{
            if(_eventModel.ShowDialog() == true)
            {
                Console.WriteLine("Accept!!!");
                EmoEngine.Instance.MentalCommandSetTrainingControl(0, EdkDll.IEE_MentalCommandTrainingControl_t.MC_ACCEPT);
                _eventModel.UpdateTrainedActions(true);
            }
            else
            {
                EmoEngine.Instance.MentalCommandSetTrainingControl(0, EdkDll.IEE_MentalCommandTrainingControl_t.MC_REJECT);
            }
            _eventModel.TrainingStatus = false;
        }

        public void engine_MentalCommandTrainingCompleted(object sender, EmoEngineEventArgs e)
        {
            Console.WriteLine("MentalCommand Training Completed.");
        }

        public void engine_MentalCommandTrainingRejected(object sender, EmoEngineEventArgs e)
        {
            Console.WriteLine("MentalCommand Training Rejected.");
        }
        #endregion

        #region MentalTraining
        public void TrainNeutral()
        {
            EmoEngine.Instance.MentalCommandSetTrainingAction(0, EdkDll.IEE_MentalCommandAction_t.MC_NEUTRAL);
            EmoEngine.Instance.MentalCommandSetTrainingControl(0, EdkDll.IEE_MentalCommandTrainingControl_t.MC_START);
            _eventModel.CurrentTrainedAction = 1;
        }

        public void TrainPull()
        {
            EmoEngine.Instance.MentalCommandSetActiveActions(0, (uint)EdkDll.IEE_MentalCommandAction_t.MC_PULL);
            EmoEngine.Instance.MentalCommandSetTrainingAction(0, EdkDll.IEE_MentalCommandAction_t.MC_PULL);
            EmoEngine.Instance.MentalCommandSetTrainingControl(0, EdkDll.IEE_MentalCommandTrainingControl_t.MC_START);
            _eventModel.CurrentTrainedAction = 4;
        }

        public void TrainPush()
        {
            EmoEngine.Instance.MentalCommandSetActiveActions(0, (uint)EdkDll.IEE_MentalCommandAction_t.MC_PUSH);
            EmoEngine.Instance.MentalCommandSetTrainingAction(0, EdkDll.IEE_MentalCommandAction_t.MC_PUSH);
            EmoEngine.Instance.MentalCommandSetTrainingControl(0, EdkDll.IEE_MentalCommandTrainingControl_t.MC_START);
            _eventModel.CurrentTrainedAction = 2;
        }

        public void TrainLeft()
        {
            EmoEngine.Instance.MentalCommandSetActiveActions(0, (uint)EdkDll.IEE_MentalCommandAction_t.MC_LEFT);
            EmoEngine.Instance.MentalCommandSetTrainingAction(0, EdkDll.IEE_MentalCommandAction_t.MC_LEFT);
            EmoEngine.Instance.MentalCommandSetTrainingControl(0, EdkDll.IEE_MentalCommandTrainingControl_t.MC_START);
            _eventModel.CurrentTrainedAction = 32;
        }

        public void TrainRight()
        {
            EmoEngine.Instance.MentalCommandSetActiveActions(0, (uint)EdkDll.IEE_MentalCommandAction_t.MC_RIGHT);
            EmoEngine.Instance.MentalCommandSetTrainingAction(0, EdkDll.IEE_MentalCommandAction_t.MC_RIGHT);
            EmoEngine.Instance.MentalCommandSetTrainingControl(0, EdkDll.IEE_MentalCommandTrainingControl_t.MC_START);
            _eventModel.CurrentTrainedAction = 64;
        }
        #endregion
    }
}
