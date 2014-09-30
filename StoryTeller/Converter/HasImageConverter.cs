using StoryTeller.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;

namespace StoryTeller.Converter
{
    public sealed class HasImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool negate = false;
            if (null != parameter)
            {
                bool.TryParse(parameter.ToString(), out negate);
            }

            bool result = false;
            RichTextBlock richBlock = new StoryTeller.Converter.StringToRtf().Convert(value.ToString(), null, null, null) as RichTextBlock;
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
                                result = true;
                                break;
                            }
                        }
                    }
                }
            }

            if (!negate)
            {
                return (result) ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return (result) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class HasTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool negate = false;
            if (null != parameter)
            {
                bool.TryParse(parameter.ToString(), out negate);
            }

            bool result = false;
            RichTextBlock richBlock = new StoryTeller.Converter.StringToRtf().Convert(value.ToString(), null, null, null) as RichTextBlock;
            if (null != richBlock)
            {
                foreach (Block block in richBlock.Blocks)
                {
                    Paragraph p = block as Paragraph;
                    if (null != p)
                    {
                        foreach (Inline inline in p.Inlines)
                        {
                            Run run;
                            if (ImageInline.IsImageInline(inline))
                            {
                                continue;
                            }
                            else if (null != (run = inline as Run))
                            {
                                if (!string.IsNullOrWhiteSpace(run.Text))
                                {
                                    result = true;
                                }
                            }
                        }
                    }
                }
            }

            if (!negate)
            {
                return (result) ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return (result) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
