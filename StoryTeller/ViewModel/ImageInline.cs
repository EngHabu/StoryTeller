using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace StoryTeller.ViewModel
{
    internal interface IRichContentProvider
    {
        string XamlString { get; }
        string PlainText { get; }
    }

    internal class ImageInline
    {
        private const string PlainTextFormat = "[[File:{0}|{1}]]";
        private static Regex _matchExpression = new Regex(
            @"(\[\[file:(?<FilePath>[^\u007c]*)\u007c((?<Options>[^\u007c\]]*)\u007c)*(?<Caption>[^\]\u007c]*)\]\])",
            RegexOptions.IgnoreCase | RegexOptions.Multiline);

        public string FilePath { get; set; }
        public string Caption { get; set; }

        public static ImageInline Create(Inline inline)
        {
            if (null == inline)
            {
                throw new ArgumentNullException("inline");
            }

            if (!IsImageInline(inline))
            {
                throw new ArgumentException("Inline is not an image inline", "inline");
            }

            ImageInline result;
            if (!TryCreate(inline, out result))
            {
                throw new InvalidOperationException("Failed to create image inline");
            }

            return result;
        }

        public static bool TryCreate(Inline inline, out ImageInline imageInline)
        {
            imageInline = null;
            if (null == inline)
            {
                return false;
            }

            if (!IsImageInline(inline))
            {
                return false;
            }

            Span span = inline as Span;
            Run imagePath = span.Inlines[0] as Run;
            Run caption = span.Inlines[1] as Run;

            imageInline = new ImageInline
            {
                FilePath = imagePath.Text,
                Caption = caption.Text
            };

            return true;
        }

        public static void FixUpXaml(Paragraph paragraph)
        {
            for(int i = 0; i < paragraph.Inlines.Count; i++)
            {
                Inline inline = paragraph.Inlines[i];
                ImageInline imageInline;
                if (TryCreate(inline, out imageInline))
                {
                    BitmapImage bi = new BitmapImage(new Uri(imageInline.FilePath));
                    Image image = new Image();
                    image.Source = bi;
                    InlineUIContainer container = new InlineUIContainer();
                    image.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                    image.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                    container.Child = image;
                    paragraph.Inlines[i] = container;

                    // if this is an image only paragraph. Center it
                    if (paragraph.Inlines.Count == 1)
                    {
                        paragraph.TextAlignment = Windows.UI.Xaml.TextAlignment.Center;
                    }
                }
            }
        }

        public static bool IsImageInline(Inline inline)
        {
            bool isImageSpan = IsImageSpan(inline);
            bool isImageUIInline = IsImageInlineUIElement(inline);

            return isImageSpan || isImageUIInline;
        }

        private static bool IsImageInlineUIElement(Inline inline)
        {
            InlineUIContainer container = inline as InlineUIContainer;
            return null != container
                && container.Child.GetType() == typeof(Image);
        }

        private static bool IsImageSpan(Inline inline)
        {
            Span span = inline as Span;
            bool isImageSpan = null != span
                && inline.GetType() == typeof(Span)
                && span.Inlines.Count == 2
                && InvisibleRun.IsInvisible(span.Inlines[0] as Run)
                && !InvisibleRun.IsInvisible(span.Inlines[1] as Run);
            return isImageSpan;
        }

        public static string ToXamlString(string plainText)
        {
            plainText = _matchExpression.Replace(plainText, "<Span>" + InvisibleRun.Create("{}${FilePath}").XamlString + "<Run Text=\"${Caption}\" /></Span>");
            return plainText;
        }

        internal static string ToPlainText(Inline inline)
        {
            string result = string.Empty;
            ImageInline imageInline;
            if (TryCreate(inline, out imageInline))
            {
                result = string.Format(PlainTextFormat, imageInline.FilePath, imageInline.Caption);
            }

            return result;
        }
    }
}
