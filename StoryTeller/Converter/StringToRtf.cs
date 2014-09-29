using StoryTeller.Controls;
using StoryTeller.DataModel.Model;
using StoryTeller.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace StoryTeller.Converter
{
    public sealed class StringToRtf : IValueConverter
    {
        private const double InvisibleFontSize = 0.004;
        private const double epsilon = 0.0001;

        private static bool IsInvisibleFontSize(double fontSize)
        {
            return Math.Abs(fontSize - InvisibleFontSize) < epsilon;
        }

        public static string PlainTextToRtf(string plainText)
        {
            string escapedPlainText = plainText.Replace(@"\", @"\\").Replace("{", @"\{").Replace("}", @"\}");
            string rtf = @"{\rtf1\ansi{\fonttbl\f0\fswiss Helvetica;}\f0\pard ";
            rtf += escapedPlainText.Replace(Environment.NewLine, @" \par ");
            rtf += " }";
            return rtf;
        }

        public static string PlainTextToXaml(string plainText)
        {
            return @"<Paragraph TextIndent=""20"">"
                + ConvertImagesToXamlString(ConvertLinksToXamlString(plainText)).Replace(Environment.NewLine, @"</Paragraph><Paragraph><Run Text="""" FontSize=""" + InvisibleFontSize + @"""/></Paragraph><Paragraph TextIndent=""20"">")
                + "</Paragraph>";
        }

        private static string ConvertLinksToXamlString(string plainText)
        {
            plainText = Regex.Replace(plainText, @"(\{(?<SceneId>[^}]*):(?<LinkText>[^}]*)\})",
                @"<Hyperlink><Run Text=""#$2"" FontSize=""" + InvisibleFontSize + @"""/><Run Text=""$3""/></Hyperlink>");
            return plainText;
        }

        private static string ConvertImagesToXamlString(string plainText)
        {
            return ImageInline.ToXamlString(plainText);
        }

        public static IEnumerable<Block> PlainTextToBlocks(string plainText)
        {
            RichTextBlock richTextBlock = PlainTextToRichTextBlock(plainText);
            List<Block> result = new List<Block>();
            while (richTextBlock.Blocks.Count > 0)
            {
                Block block = richTextBlock.Blocks.First();
                richTextBlock.Blocks.Remove(block);
                result.Add(block);
            }

            return result.AsEnumerable();
        }

        public static RichTextBlock PlainTextToRichTextBlock(string plainText)
        {
            RichTextBlock blocksObj = XamlReader.Load(
                @"<RichTextBlock TextWrapping=""Wrap"" Padding=""20"" Width=""750"" Height=""700"" CharacterSpacing=""100"" LineHeight=""20"" MaxLines=""31"" FontSize=""20"" FontFamily=""Times New Roman"" xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>"
                + PlainTextToXaml(plainText)
                + "</RichTextBlock>") as RichTextBlock;
            blocksObj.ContextMenuOpening += mainBlock_ContextMenuOpening;

            return blocksObj;
        }

        private void FixImages(RichTextBlock rtbBlock)
        {
            foreach (Block block in rtbBlock.Blocks)
            {
                Paragraph paragraph = block as Paragraph;
                if (null != paragraph)
                {
                    FixImages(paragraph);
                }
            }
        }

        private void FixImages(Paragraph paragraph)
        {
            if (null == paragraph)
            {
                throw new ArgumentNullException("paragraph");
            }

            ImageInline.FixUpXaml(paragraph);
        }

        public static void FixHyperLinks(RichTextBlock rtbBlock, Action<Hyperlink, string> clickAction)
        {
            foreach (Block block in rtbBlock.Blocks)
            {
                Paragraph paragraph = block as Paragraph;
                if (null != paragraph)
                {
                    FixHyperLinks(paragraph, clickAction);
                }
            }
        }

        private static void FixHyperLinks(Paragraph paragraph, Action<Hyperlink, string> clickAction)
        {
            if (null == paragraph)
            {
                throw new ArgumentNullException("paragraph");
            }

            foreach (Inline inline in paragraph.Inlines)
            {
                Hyperlink link = inline as Hyperlink;
                if (null != link)
                {
                    string linkId = string.Empty;
                    if (null != link.NavigateUri)
                    {
                        linkId = link.NavigateUri.OriginalString.Substring(link.NavigateUri.OriginalString.LastIndexOf('/') + 1);
                        link.NavigateUri = null;
                    }
                    else
                    {
                        linkId = string.Empty;
                        foreach (Inline linkInline in link.Inlines)
                        {
                            Run linkRun = linkInline as Run;
                            if (IsInvisibleFontSize(linkRun.FontSize))
                            {
                                linkId = linkRun.Text;
                            }
                        }
                    }

                    link.Click += (hyperlink, args) =>
                    {
                        clickAction(hyperlink, linkId);
                    };
                }
            }
        }


        static void link_Click(Hyperlink sender, string linkId)
        {
            RichTextBlock parentRichTextBlock = VisualTreeHelper.GetParent(sender) as RichTextBlock;
            if (null != parentRichTextBlock)
            {
                SceneViewModel scene = parentRichTextBlock.DataContext as SceneViewModel;
                if (null != scene)
                {
                    scene.LinkClicked(linkId);
                }
            }
        }

        private static void RaiseCreateSceneLinkEvent(TextPointer start, TextPointer end)
        {
            FrameworkElement element = start.VisualParent;
            if (null != element)
            {
                SceneViewModel scene = element.DataContext as SceneViewModel;
                if (null != scene)
                {
                    scene.CreateLinkAt(start, end);
                }
            }
        }

        private static Rect GetVisualRectangle(RichTextBlock textBlock, TextPointer textPointer)
        {
            UIElement container = textPointer.VisualParent;
            Rect beforeTransform = textPointer.GetCharacterRect(LogicalDirection.Forward);
            GeneralTransform transform = container.TransformToVisual(null);
            Rect afterTransform = transform.TransformBounds(beforeTransform);
            return afterTransform;
        }

        // returns a rect for selected text
        // if no text is selected, returns caret location
        // textbox should not be empty
        private static Rect GetTextboxSelectionRect(RichTextBlock textbox)
        {
            Rect rectFirst, rectLast;
            rectFirst = GetVisualRectangle(textbox, textbox.SelectionStart);
            rectLast = GetVisualRectangle(textbox, textbox.SelectionEnd);
            double left = Math.Min(rectFirst.Left, rectLast.Left);
            double top = Math.Min(rectFirst.Top, rectLast.Top);
            double right = Math.Max(rectFirst.Right, rectLast.Right);
            double bottom = Math.Max(rectFirst.Bottom, rectLast.Bottom);
            Rect boundingRect = new Rect(left, top, right - left, bottom - top);

            return boundingRect;
        }

        private static async void mainBlock_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            e.Handled = true;
            RichTextBlock textbox = (RichTextBlock)sender;

            SceneViewModel sceneModel = textbox.DataContext as SceneViewModel;
            InteractiveScene interactiveScene = sceneModel.CurrentScene as InteractiveScene;
            if (interactiveScene.Type != SceneType.Interactive || interactiveScene.PossibleScenes.Count == 0)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(textbox.SelectedText) && textbox.SelectedText.Length > 0)
            {
                Rect rect = GetTextboxSelectionRect(textbox);
                Popup popup = new Popup();                

                popup.VerticalOffset = rect.Y + rect.Height/2;
                popup.HorizontalOffset = rect.X - 5;
                popup.IsLightDismissEnabled = true;

                ScenePickerViewModel scenePickerModel = ScenePickerViewModel.Create(interactiveScene);                                
                ScenePickerControl scenePicker = new ScenePickerControl();
                scenePicker.PickSceneRequest += (IScene selectedScene) => 
                {
                    string linkId = CreateHyperlink(textbox);
                    scenePickerModel.LinkId = linkId;
                    scenePickerModel.SelectedScene = selectedScene;                    
                    popup.IsOpen = false;
                };

                scenePicker.DataContext = scenePickerModel;                                
                popup.Child = scenePicker;
                popup.IsOpen = true;
            }
        }

        private static string CreateHyperlink(RichTextBlock textbox)
        {
            TextPointer start = textbox.SelectionStart;
            TextPointer end = textbox.SelectionEnd;
            Run startRun = start.Parent as Run;
            Run endRun = end.Parent as Run;
            int startIndex = start.Offset - startRun.ElementStart.Offset;
            int endIndex = end.Offset - endRun.ElementStart.Offset;
            if (startRun == endRun)
            {
                IEnumerable<Inline> splitted = SplitRun(startRun, startIndex, endIndex);
                Paragraph p = startRun.ElementStart.Parent as Paragraph;
                int runIndex = p.Inlines.IndexOf(startRun);
                p.Inlines.RemoveAt(runIndex);
                foreach (Inline inline in splitted.Reverse())
                {
                    p.Inlines.Insert(runIndex, inline);
                }

                FixHyperLinks(p, link_Click);
                string linkId = (from inline in splitted
                                where inline is Hyperlink
                                select (from inline2 in (inline as Hyperlink).Inlines
                                        where IsInvisibleFontSize((inline2 as Run).FontSize)
                                        select (inline2 as Run).Text).FirstOrDefault()).FirstOrDefault();
                UpdateOriginalText(textbox);

                return linkId;
            }
            return null;
        }

        private static void UpdateOriginalText(RichTextBlock richTextBlock)
        {
            string plainText = ConvertRichTextBlocksToPlainText(richTextBlock.Blocks);
            SceneViewModel sceneViewModel = richTextBlock.DataContext as SceneViewModel;
            if (null != sceneViewModel)
            {
                sceneViewModel.CurrentScene.Content.Content = plainText;
            }
        }

        private static string ConvertRichTextBlocksToPlainText(BlockCollection blockCollection)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Block block in blockCollection)
            {
                bool isEmptyParagraph = true;
                Paragraph paragraph = block as Paragraph;
                foreach (Inline inline in paragraph.Inlines)
                {
                    Run run = inline as Run;
                    Hyperlink link = inline as Hyperlink;
                    if (null != run && !IsInvisibleFontSize(run.FontSize))
                    {
                        isEmptyParagraph = false;
                        sb.Append(run.Text);
                    }
                    else if (null != link)
                    {
                        isEmptyParagraph = false;
                        string linkContents = string.Empty;
                        string linkId = string.Empty;
                        foreach (Inline linkInline in link.Inlines)
                        {
                            Run linkRun = linkInline as Run;
                            if (IsInvisibleFontSize(linkRun.FontSize))
                            {
                                linkId = linkRun.Text.Substring(linkRun.Text.IndexOf('#') + 1);
                            }
                            else
                            {
                                linkContents = linkRun.Text;
                            }
                        }

                        sb.AppendFormat("{{{0}:{1}}}",
                            linkId, linkContents);
                    }
                    else if (ImageInline.IsImageInline(inline))
                    {
                        isEmptyParagraph = false;
                        sb.Append(ImageInline.ToPlainText(inline));
                    }
                }

                if (!isEmptyParagraph)
                {
                    sb.AppendLine();
                }
            }

            string result = sb.ToString();
            return result;
        }

        private static IEnumerable<Inline> SplitRun(Run startRun, int startIndex, int endIndex)
        {
            string original = startRun.Text;
            string before = original.Substring(0, startIndex - 1);
            string mid = original.Substring(startIndex - 1, endIndex - startIndex);
            // TODO: haytham: Will crash if endIndex is last character...
            string after = original.Substring(endIndex - 1);
            List<Inline> inlines = new List<Inline>();
            inlines.Add(new Run { Text = before });
            inlines.Add(CreateHyperlink(mid, Guid.NewGuid().ToString()));
            inlines.Add(new Run { Text = after });
            return inlines;
        }

        private static Hyperlink CreateHyperlink(string content, string navigateUri)
        {
            Hyperlink link = new Hyperlink();
            link.Inlines.Add(new Run { Text = "#" + navigateUri, FontSize = InvisibleFontSize });
            link.Inlines.Add(new Run { Text = content });
            return link;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string rtfText = (string)value;
            RichTextBlock blocksObj = PlainTextToRichTextBlock(rtfText);
            FixHyperLinks(blocksObj, link_Click);
            FixImages(blocksObj);
            return blocksObj;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
