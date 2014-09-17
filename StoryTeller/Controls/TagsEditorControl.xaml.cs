using StoryTeller.DataModel.Model;
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
    public sealed partial class TagsEditorControl : UserControl
    {
        public TagsEditorControl()
        {
            this.InitializeComponent();
            tagTypes.ItemsSource = Enum.GetValues(typeof(SceneTagType)).Cast<SceneTagType>();
            tagTypes.SelectedItem = SceneTagType.Custom;
        }

        private void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                SceneViewModel sceneModel = DataContext as SceneViewModel;
                sceneModel.Tags.Add(new SceneTag(tagTypes.SelectedItem.ToString(), tagValue.Text));
            }
        }
    }
}
