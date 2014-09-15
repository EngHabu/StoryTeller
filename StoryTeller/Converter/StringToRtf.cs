using StoryTeller.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace StoryTeller.Converter
{
    public sealed class StringToRtf : IValueConverter
    {
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
                + ConvertLinksToXamlString(plainText).Replace(Environment.NewLine, @"</Paragraph><Paragraph TextIndent=""20"">")
                + "</Paragraph>";
        }

        private static string ConvertLinksToXamlString(string plainText)
        {
            plainText = Regex.Replace(plainText, @"(\{(?<SceneId>[^}]*):(?<LinkText>[^}]*)\})",
                @"<Hyperlink NavigateUri=""#$2""><Run Text=""$3""/></Hyperlink>");
            return plainText;
        }

        public static BlockCollection PlainTextToBlockCollection(string plainText)
        {
            string innerText = PlainTextToXaml(plainText);
            return XamlReader.Load("<BlockCollection  xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>"
                + innerText
                + "</BlockCollection>") as BlockCollection;
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
                @"<RichTextBlock TextWrapping=""Wrap"" Padding=""20"" Width=""750"" Height=""700"" CharacterSpacing=""100"" LineHeight=""20"" MaxLines=""31"" FontSize=""14"" FontFamily=""Times New Roman"" xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>"
                + PlainTextToXaml(plainText)
                + "</RichTextBlock>") as RichTextBlock;

            return blocksObj;
        }

        public static void FixHyperLinks(RichTextBlock rtbBlock, Action<Hyperlink, string> clickAction)
        {
            foreach (Block block in rtbBlock.Blocks)
            {
                Paragraph paragraph = block as Paragraph;
                if (null != paragraph)
                {
                    foreach (Inline inline in paragraph.Inlines)
                    {
                        Hyperlink link = inline as Hyperlink;
                        if (null != link)
                        {
                            string linkId = link.NavigateUri.OriginalString.Substring(link.NavigateUri.OriginalString.LastIndexOf('/') + 1);
                            link.NavigateUri = null;
                            link.Click += (hyperlink, args) =>
                            {
                                clickAction(hyperlink, linkId);
                            };
                        }
                    }
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

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string rtfText = (string)value;
            RichTextBlock blocksObj = PlainTextToRichTextBlock(rtfText);
            FixHyperLinks(blocksObj, link_Click);
            return blocksObj;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
