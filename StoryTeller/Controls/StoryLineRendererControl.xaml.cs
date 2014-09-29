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
    public sealed partial class StoryLineRendererControl : UserControl
    {
        public StoryLineRendererControl()
        {
            this.InitializeComponent();
        }

        private void ListBox_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            
        }

        private void storylinePanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StoryLineViewModel storylineModel = storylinePanel.SelectedItem as StoryLineViewModel;
            if (null != storylineModel && null != storylineModel.StoryModel)
            {
            storylineModel.StoryModel.SelectStoryline(storylineModel);
        }
    }
}
}
