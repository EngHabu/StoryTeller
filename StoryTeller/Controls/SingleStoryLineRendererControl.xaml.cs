﻿using StoryTeller.ViewModel;
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
    public sealed partial class SingleStoryLineRendererControl : UserControl
    {
        public SingleStoryLineRendererControl()
        {
            this.InitializeComponent();
        }

        private void addStoryline_Click(object sender, RoutedEventArgs e)
        {
            StoryLineViewModel storyline = (DataContext as StoryLineViewModel);
            StoryViewModel storyModel = storyline.StoryModel;
            storyModel.AddStoryline(storyline);
        }

        private void selectStoryline_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StoryLineViewModel storyline = (DataContext as StoryLineViewModel);
            StoryViewModel storyModel = storyline.StoryModel;
            storyModel.SelectStoryline(storyline);
        }
    }
}
