using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emotiv;
using System.Diagnostics;
using System.Windows.Media;

namespace SP_EEG
{
    public static class Globals
    {
        #region Settings

        public static string SettingsFile = "Settings.ini";

        #endregion

        #region Strings
        public static string TrainingQuestinText = "Do you wish to accept the training?";
        public static string CogNeutral = "Neutral";
        public static string CogPush = "Push";
        public static string CogPull = "Pull";
        public static string CogLeft = "Left";
        public static string CogRight = "Right";
        public static string DefaultPort = "COM3";
        public static string EASY = "easy";
        public static string SHARP = "sharp";
        public static string NORMAL = "normal";
        public static string FAST = "fast";
        public static string SLOW = "slow";
        public static string CONNECTED_STATUS = "Status: Connected";
        public static string DISCONNECTED_STATUS = "Status: Disconnected";
        public static string LINE_COLOR = "Value: ";
        public static string BORDER_COLOR = "Value: ";
        #endregion

        #region Robot Constants
        public static int FORWARD = 20;
        public static int BACKWARDS = -30;
        public static uint TIME = 40;
        public static uint TIMELONG = 3000;
        public static int COLOR_THRESHOLD = 65;//35;//65;
        public static double TURN_COEFFICIENT = 1.3;
        public static double FORWARD_POWER_SCALE = 0.6;
        public static double TURN_POWER_SCALE = 0.5;
        public static int MAX_ATTENTION = 60;
        public static int MIN_ATTENTION = 0;

        public static int MAX_COLOR_TRESHOLD = 100;
        public static int MIN_COLOR_TRESHOLD = 0;
        public static double MAX_TURN_COEFFICIENT = 5;
        public static double MIN_TURN_COEFFICIENT = 0;
        public static double MAX_FORWARD_POWER_SCALE = 1;
        public static double MIN_FORWARD_POWER_SCALE = 0;
        public static double MAX_TURN_POWER_SCALE = 1;
        public static double MIN_TURN_POWER_SCALE = 0;
        #endregion

        #region Colors
        public static SolidColorBrush Black = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        public static SolidColorBrush White = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        public static SolidColorBrush BrightGreen = new SolidColorBrush(Color.FromRgb(0, 255, 0));
        public static SolidColorBrush BrightYellow = new SolidColorBrush(Color.FromRgb(255, 255, 0));
        public static SolidColorBrush DarkYellow = new SolidColorBrush(Color.FromRgb(255, 190, 0));
        public static SolidColorBrush Red = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        public static SolidColorBrush Blue = new SolidColorBrush(Color.FromRgb(0, 0, 255));
        public static SolidColorBrush BrightBlue = new SolidColorBrush(Color.FromRgb(65, 107, 173));

        public static SolidColorBrush SchemeBlack = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2B2B2B"));
        public static SolidColorBrush SchemeRed = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DE1B1B"));
        public static SolidColorBrush SchemeWhite = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F6F6F6"));
        public static SolidColorBrush SchemeYellow = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E9E581"));

        public static SolidColorBrush DarkestRuby = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9A2617"));
        public static SolidColorBrush DarkerRuby = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AD2A1A"));
        public static SolidColorBrush Ruby = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C02F1D"));
        public static SolidColorBrush LighterRuby = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CD594A"));
        public static SolidColorBrush GrassGreen = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#005A31"));
        
        #endregion
    }
}
