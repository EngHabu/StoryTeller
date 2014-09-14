using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace StoryTeller.Converter
{
    public sealed class XamlToRichTextBlockCollection : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ObservableCollection<UIElement> blocks = new ObservableCollection<UIElement>();
            ObservableCollection<IScene> scenes = value as ObservableCollection<IScene>;
            double zoomFactor = 1;
            if (null != parameter)
            {
                zoomFactor = (double)parameter;
            }

            FillInBlocks(scenes, blocks, zoomFactor);

            scenes.CollectionChanged += (p1, p2) =>
                {
                    blocks.Clear();
                    FillInBlocks(scenes, blocks, zoomFactor);
                };

            return blocks;
        }

        private static void FillInBlocks(ObservableCollection<IScene> scenes, ObservableCollection<UIElement> blocks, double zoomFactor)
        {
            double width = 750 * zoomFactor;
            double height = 700 * zoomFactor;
            foreach (IScene scene in scenes)
            {
                string stringContent = scene.Content.Content;
                RichTextBlock mainBlock = new StringToRtf().Convert(stringContent, null, null, null) as RichTextBlock;
                mainBlock.DataContext = scene;
                mainBlock.Padding = new Thickness(20);
                mainBlock.Foreground = new SolidColorBrush(Colors.Black);
                mainBlock.Measure(new Windows.Foundation.Size(width, height));
                blocks.Add(mainBlock);
                if (mainBlock.HasOverflowContent)
                {
                    RichTextBlockOverflow overflow = new RichTextBlockOverflow()
                    {
                        Width = width,
                        Height = height
                    };

                    overflow.DataContext = scene;

                    mainBlock.OverflowContentTarget = overflow;
                    overflow.Padding = new Thickness(20);
                    overflow.Measure(new Windows.Foundation.Size(width, height));
                    blocks.Add(overflow);
                    while (overflow.HasOverflowContent)
                    {
                        RichTextBlockOverflow nextOverflow = new RichTextBlockOverflow()
                        {
                            Width = width,
                            Height = height
                        };

                        nextOverflow.DataContext = scene;

                        overflow.OverflowContentTarget = nextOverflow;
                        nextOverflow.Padding = new Thickness(20);
                        nextOverflow.Measure(new Windows.Foundation.Size(width, height));
                        blocks.Add(nextOverflow);
                        overflow = nextOverflow;
                    }
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
