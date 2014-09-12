using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace StoryTeller.Converter
{
    public sealed class XamlToRichTextBlockCollection : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            List<UIElement> blocks = new List<UIElement>();
            IEnumerable<IScene> valueString = value as IEnumerable<IScene>;
            FlipView flipView = parameter as FlipView;
            foreach (IScene scene in valueString)
            {
                string stringContent = scene.Content.Content;
                RichTextBlock mainBlock = new StringToRtf().Convert(stringContent, null, null, null) as RichTextBlock;
                mainBlock.Measure(new Windows.Foundation.Size(750, 700));
                //RichTextBlock mainBlock = value as RichTextBlock;
                blocks.Add(mainBlock);
                if (mainBlock.HasOverflowContent)
                {
                    RichTextBlockOverflow overflow = new RichTextBlockOverflow()
                    {
                        Width = 750,
                        Height = 700
                    };

                    mainBlock.OverflowContentTarget = overflow;
                    overflow.Measure(new Windows.Foundation.Size(750, 700));
                    blocks.Add(overflow);
                    while (overflow.HasOverflowContent)
                    {
                        RichTextBlockOverflow nextOverflow = new RichTextBlockOverflow()
                        {
                            Width = 750,
                            Height = 700
                        };
                        overflow.OverflowContentTarget = nextOverflow;
                        nextOverflow.Measure(new Windows.Foundation.Size(750, 700));
                        blocks.Add(nextOverflow);
                        overflow = nextOverflow;
                    }
                }
            }

            return blocks.AsEnumerable();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
