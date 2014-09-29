using StoryTeller.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace StoryTeller.Converter
{
    public sealed class ImageFromText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ImageSource result = null;
            string content = value as string;
            if (null != content)
            {
                RichTextBlock richBlock = new StoryTeller.Converter.StringToRtf().Convert(content, null, null, null) as RichTextBlock;
                if (null != richBlock)
                {
                    foreach (Block block in richBlock.Blocks)
                    {
                        Paragraph p = block as Paragraph;
                        if (null != p)
                        {
                            foreach (Inline inline in p.Inlines)
                            {
                                if (ImageInline.IsImageInline(inline))
                                {
                                    InlineUIContainer container = inline as InlineUIContainer;
                                    Image imageChild = null;
                                    if (null == container)
                                    {
                                    }
                                    else if (null == (imageChild = container.Child as Image))
                                    {
                                    }
                                    else
                                    {
                                        result = imageChild.Source;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
