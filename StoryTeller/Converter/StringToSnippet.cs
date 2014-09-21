using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace StoryTeller.Converter
{
    public sealed class StringToSnippet : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string content = ((string)value);
            return content == null ? "" : content.Substring(0, content.Length > 10 ? 10 : content.Length) + "...";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
