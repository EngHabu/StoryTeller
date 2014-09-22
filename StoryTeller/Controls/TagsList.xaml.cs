using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class TagsList : UserControl
    {
        public TagsList()
        {
            this.InitializeComponent();
        }

        public Brush TagColor
        {
            get { return (Brush)GetValue(TagColorProperty); }
            set { SetValue(TagColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TagColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TagColorProperty =
            DependencyProperty.Register("TagColor", typeof(Brush), typeof(TagsList), new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));

        public SceneTag SelectedTag
        {
            get
            {
                return tagsList.SelectedItem as SceneTag;
            }
        }
    }
}
