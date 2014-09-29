using StoryTeller.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace StoryTeller.Converter
{
    public class CurrentStorylineToColor : IValueConverter
    {
        private SolidColorBrush _greenYellow = new SolidColorBrush(Colors.GreenYellow);
        private SolidColorBrush _transparent = new SolidColorBrush(Colors.Transparent);
        public object Convert(object value, Type targetType, object parameter, string language)
        {           
            return ((bool)value) ? _greenYellow : _transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
