using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;

namespace StoryTeller.ViewModel
{
    internal sealed class InvisibleRun
    {
        private const double InvisibleFontSize = 0.004;
        private const double epsilon = 0.0001;
        private Run _run = new Run();

        public Run Run
        {
            get { return _run; }
            set { _run = value; }
        }

        public string XamlString
        {
            get
            {
                return "<Run FontSize=\"" + Run.FontSize + "\" Text=\"" + Run.Text + "\" />";
            }
        }

        private InvisibleRun()
        {
        }

        private static bool IsInvisibleFontSize(double fontSize)
        {
            return Math.Abs(fontSize - InvisibleFontSize) < epsilon;
        }

        public static InvisibleRun Create(string content)
        {
            InvisibleRun result = new InvisibleRun
            {
                Run = new Run
                {
                    FontSize = InvisibleFontSize,
                    Text = content
                }
            };

            return result;
        }

        public static InvisibleRun Create(Run run)
        {
            return Create(run.Text);
        }

        public static bool IsInvisible(Run run)
        {
            return IsInvisibleFontSize(run.FontSize);
        }
    }
}
