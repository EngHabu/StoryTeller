﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

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
                + plainText.Replace(Environment.NewLine, @"</Paragraph><Paragraph TextIndent=""20"">") 
                + "</Paragraph>";
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string rtfText = (string)value;
            object blocksObj = XamlReader.Load(
                @"<RichTextBlock TextWrapping=""Wrap"" Padding=""20"" Width=""750"" Height=""700"" CharacterSpacing=""100"" LineHeight=""20"" MaxLines=""31"" FontSize=""14"" FontFamily=""Times New Roman"" xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>"
                + PlainTextToXaml(rtfText)
                + "</RichTextBlock>");
            return (RichTextBlock)blocksObj;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
