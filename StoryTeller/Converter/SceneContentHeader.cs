using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace StoryTeller.Converter
{
    public class SceneContentHeader : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //ISceneContentHolder sceneContent = value as ISceneContentHolder;
            string stringContent = value as string;// sceneContent.Content;
            if (stringContent != null)
            {
                string header = stringContent.Length > 15 ? stringContent.Substring(0, 15) : stringContent;
                if(header != null && header.EndsWith(".txt"))
                {
                    header = header.Substring(0, header.Length-4);
                }
                return header;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
