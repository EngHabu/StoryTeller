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
    public sealed partial class SingleTagRenderer : UserControl
    {
        public Brush TagColor
        {
            get { return (Brush)GetValue(TagColorProperty); }
            set
            {
                SetValue(TagColorProperty, value);
                backgroundPanel.Fill = value;
            }
        }

        // Using a DependencyProperty as the backing store for TagColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TagColorProperty =
            DependencyProperty.Register("TagColor", typeof(Brush), typeof(SingleTagRenderer), new PropertyMetadata(0));


        public SingleTagRenderer()
        {
            this.InitializeComponent();           
        }
    }
}
