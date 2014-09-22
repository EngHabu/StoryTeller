using StoryTeller.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace StoryTeller.Controls
{
    public sealed partial class PagesControl : ItemsControl
    {
        private double _pageWidth = 750;
        private double _pageHeight = 500;
        private const double _pageWidthDiffEpsilon = 0.001;

        public int ColumnsCount
        {
            get { return (int)GetValue(ColumnsCountProperty); }
            set { SetValue(ColumnsCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColumnsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnsCountProperty =
            DependencyProperty.Register("ColumnsCount", typeof(int), typeof(PreviewRendererControl), new PropertyMetadata(0));
        
        public PagesControl()
        {
            this.InitializeComponent();
            ColumnsCount = 1;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            StoryViewModel storyViewModel = DataContext as StoryViewModel;
            if (storyViewModel != null 
                && (!AreEqual(availableSize.Width / ColumnsCount, storyViewModel.PageWidth)
                    || !AreEqual(availableSize.Height, storyViewModel.PageHeight)))
            {
                _pageHeight = storyViewModel.PageHeight = availableSize.Height;
                _pageWidth = storyViewModel.PageWidth = availableSize.Width / ColumnsCount;
                RebindView();
            }

            return base.MeasureOverride(availableSize);
        }

        private static bool AreEqual(double pageWidth1, double pageWidth2)
        {
            return Math.Abs(pageWidth1 - pageWidth2) < _pageWidthDiffEpsilon;
        }

        private void RebindView()
        {
            object currentDataContext = DataContext;
            DataContext = null;
            DataContext = currentDataContext;
        }
    }
}
