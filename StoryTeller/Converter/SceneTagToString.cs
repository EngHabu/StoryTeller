using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace StoryTeller.Converter
{
    public class SceneTagToString : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            SceneTag sceneTag = value as SceneTag;
            return sceneTag.Name + ": " + sceneTag.Content;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
