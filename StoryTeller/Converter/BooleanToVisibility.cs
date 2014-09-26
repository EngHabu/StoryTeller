using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace StoryTeller.Converter
{
    public class BooleanToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool booleanValue = (bool)value;
            bool negate = false;
            if (parameter != null)
            {
                negate = Boolean.Parse((string)parameter);
            }
            if (negate)
            {
                return booleanValue ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            { 
                return booleanValue ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
