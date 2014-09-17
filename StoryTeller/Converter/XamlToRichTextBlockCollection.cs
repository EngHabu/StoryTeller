using StoryTeller.DataModel.Model;
using StoryTeller.ViewModel;
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
        public const double DefaultPageWidth = 750;
        public const double DefaultPageHeight = 700;
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            StoryViewModel storyViewModel = value as StoryViewModel;
            ObservableCollection<UIElement> blocks = new ObservableCollection<UIElement>();
            ObservableCollection<SceneViewModel> scenes = storyViewModel.ScenesViewModel;

            FillInBlocks(scenes, blocks, storyViewModel.PageWidth, storyViewModel.PageHeight);

            scenes.CollectionChanged += (p1, p2) =>
                {
                    blocks.Clear();
                    FillInBlocks(scenes, blocks, storyViewModel.PageWidth, storyViewModel.PageHeight);
                };

            return blocks;
        }

        private static void FillInBlocks(ObservableCollection<SceneViewModel> scenes, ObservableCollection<UIElement> blocks, double width, double height)
        {
            foreach (SceneViewModel sceneViewModel in scenes)
            {
                if (sceneViewModel is SceneViewModelPad)
                {
                    continue;
                }

                IScene scene = sceneViewModel.CurrentScene;
                string stringContent = scene.Content.Content;
                RichTextBlock mainBlock = new StringToRtf().Convert(stringContent, null, null, null) as RichTextBlock;
                mainBlock.Width = width;
                mainBlock.Height = height;
                mainBlock.DataContext = sceneViewModel;
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

                    overflow.DataContext = sceneViewModel;
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

                        nextOverflow.DataContext = sceneViewModel;
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
            return value;
        }
    }
}
