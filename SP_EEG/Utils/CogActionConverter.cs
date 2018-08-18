using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SP_EEG
{
    public class CogActionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (System.Convert.ToInt32(value))
            {
                case 1:
                    return Globals.CogNeutral;
                case 2:
                    return Globals.CogPush;
                case 4:
                    return Globals.CogPull;
                case 32:
                    return Globals.CogLeft;
                case 64:
                    return Globals.CogRight;
            }
            return "None";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {
                switch (value.ToString())
                {
                    case "Neutral":
                        return 1;
                    case "Push":
                        return 2;
                    case "Pull":
                        return 4;
                    case "Left":
                        return 32;
                    case "Right":
                        return 64;
                }
            }
            return "None";
        }
    }
}
