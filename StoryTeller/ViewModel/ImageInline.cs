using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;

namespace StoryTeller.ViewModel
{
    internal sealed class ImageInline
    {
        private Regex _matchExpression = new Regex(
            @"(\[\[File:(?<FilePath>[^\u007c]*)\u007c((?<Options>[^\u007c\]]*)\u007c)*(?<Caption>[^\]\u007c]*)\]\])",
            RegexOptions.IgnoreCase | RegexOptions.Multiline);

        public void FixUpXaml(Paragraph paragraph)
        {
            foreach (Inline inline in paragraph.Inlines)
            {
                if (IsImageInline(inline))
                {

                }
            }
        }

        public bool IsImageInline(Inline inline)
        {
            Span span = inline as Span;
            return null != span
                && span.Inlines.Count == 2
                && InvisibleRun.IsInvisible(span.Inlines[0] as Run)
                && !InvisibleRun.IsInvisible(span.Inlines[1] as Run);
        }

        public string ToXamlString(string plainText)
        {
            plainText = _matchExpression.Replace(plainText, "<Span>" + InvisibleRun.Create("$1").XamlString + "<Run>$3</Run></Span>");
            return plainText;
        }
    }
}
