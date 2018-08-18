using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_EEG.Utils
{
    public class SettingsManager
    {

        public void UpdateSettings(EventModel_MindWave e)
        {
            using (var sw = System.IO.File.CreateText(Globals.SettingsFile))
            {
                sw.WriteLine("[Settings]");
                sw.WriteLine("ColorTresholdValue = " + e.ColorTresholdValue);
                sw.WriteLine("TurnCoefficient = " + e.TurnCoefficientValue);
                sw.WriteLine("ForwardPowerScale = " + e.ForwardPowerScaleValue);
                sw.WriteLine("TurnPowerScale = " + e.TurnPowerScaleValue);
                sw.WriteLine("MaxAttention = " + e.MaxAttentionValue);
            }
        }

        public void ReadSettings(EventModel_MindWave e)
        {
            var lines = System.IO.File.ReadAllLines(Globals.SettingsFile);

            // strings to look for
            var clr = "ColorTresholdValue = ";
            var trnC = "TurnCoefficient = ";
            var frw = "ForwardPowerScale = ";
            var trnP = "TurnPowerScale = ";
            var maxA = "MaxAttention = ";

            // add default values
            var clrVal = Globals.COLOR_THRESHOLD;
            var trnCVal = Globals.TURN_COEFFICIENT;
            var frwVal = Globals.FORWARD_POWER_SCALE;
            var trnPVal = Globals.TURN_POWER_SCALE;
            var maxAVal = Globals.MAX_ATTENTION;

            // file is not empty and It has [Settings] property
            if (lines.Length > 0 && lines[0] == "[Settings]")
            {
                for (int i = 1; i < lines.Length; i++)
                {
                    var isOk = lines[i].IndexOf(clr);
                    if (isOk != -1)
                    {
                        clrVal = Int32.Parse(lines[i].Substring(isOk + clr.Length));
                        //clrVal = clrVal < Globals.MIN_COLOR_TRESHOLD ? Globals.MIN_COLOR_TRESHOLD : clrVal;
                        //clrVal = clrVal > Globals.MAX_COLOR_TRESHOLD ? Globals.MAX_COLOR_TRESHOLD : clrVal;
                    }

                    isOk = lines[i].IndexOf(trnC);
                    if (isOk != -1)
                    {
                        trnCVal = double.Parse(lines[i].Substring(isOk + trnC.Length), CultureInfo.InvariantCulture);
                        //trnCVal = trnCVal < Globals.MIN_TURN_COEFFICIENT ? Globals.MIN_TURN_COEFFICIENT : trnCVal;
                        //trnCVal = trnCVal > Globals.MAX_TURN_COEFFICIENT ? Globals.MAX_TURN_COEFFICIENT : trnCVal;
                    }

                    isOk = lines[i].IndexOf(frw);
                    if (isOk != -1)
                    {
                        frwVal = double.Parse(lines[i].Substring(isOk + frw.Length), CultureInfo.InvariantCulture);
                        //frwVal = frwVal < Globals.MIN_FORWARD_POWER_SCALE ? Globals.MIN_FORWARD_POWER_SCALE : frwVal;
                        //frwVal = frwVal > Globals.MAX_FORWARD_POWER_SCALE ? Globals.MAX_FORWARD_POWER_SCALE : frwVal;
                    }

                    isOk = lines[i].IndexOf(trnP);
                    if (isOk != -1)
                    {
                        trnPVal = double.Parse(lines[i].Substring(isOk + trnP.Length), CultureInfo.InvariantCulture);
                        //trnPVal = trnPVal < Globals.MIN_TURN_POWER_SCALE ? Globals.MIN_TURN_POWER_SCALE : trnPVal;
                        //trnPVal = trnPVal > Globals.MAX_TURN_POWER_SCALE ? Globals.MAX_TURN_POWER_SCALE : trnPVal;
                    }


                    isOk = lines[i].IndexOf(maxA);
                    if (isOk != -1)
                    {
                        maxAVal = Int32.Parse(lines[i].Substring(isOk + maxA.Length));
                        //maxAVal = maxAVal < Globals.MIN_ATTENTION ? Globals.MIN_ATTENTION : maxAVal;
                        //maxAVal = maxAVal > Globals.MAX_ATTENTION ? Globals.MAX_ATTENTION : maxAVal;
                    }
                }

                e.ColorTresholdValue = clrVal;
                e.ForwardPowerScaleValue = frwVal;
                e.MaxAttentionValue = maxAVal;
                e.TurnPowerScaleValue = trnPVal;
                e.TurnCoefficientValue = trnCVal;

            }
        }
    }
}
