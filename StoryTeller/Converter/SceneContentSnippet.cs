using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace StoryTeller.Converter
{
    public class SceneContentSnippet : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ISceneContentHolder sceneContent = value as ISceneContentHolder;
            string stringContent = sceneContent.Content;
            if (stringContent != null)
            {
                return stringContent.Length > 150 ? stringContent.Substring(0, 150) : stringContent;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
