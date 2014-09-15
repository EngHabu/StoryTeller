using StoryTeller.DataModel;
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
    public sealed partial class PreviewRendererControl : UserControl
    {
        public double ZoomFactor
        {
            get { return (double)GetValue(ZoomFactorProperty); }
            set { SetValue(ZoomFactorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ZoomFactor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoomFactorProperty =
            DependencyProperty.Register("ZoomFactor", typeof(double), typeof(PreviewRendererControl), new PropertyMetadata(0));

        
        public PreviewRendererControl()
        {
            this.InitializeComponent();
            DataContextChanged += PreviewRendererControl_DataContextChanged;
        }

        void PreviewRendererControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            StoryViewModel storyViewModel = args.NewValue as StoryViewModel;
            if (null != storyViewModel)
            {
                storyViewModel.PossibleScenePickRequest += storyViewModel_PossibleScenePickRequest;
            }
        }

        void storyViewModel_PossibleScenePickRequest(object sender, ScenePickerRequestArgs args)
        {
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
            {
                FlyoutBase flyoutBase = Flyout.GetAttachedFlyout(PagesControl);
                Flyout flyout = flyoutBase as Flyout;
                if (null != flyout)
                {
                    ScenePickerControl scenePicker = flyout.Content as ScenePickerControl;
                    if (null != scenePicker)
                    {
                        scenePicker.DataContext = args.SenderChain.Where((obj) =>
                            (obj is SceneViewModel)).FirstOrDefault();
                    }
                }

                flyoutBase.Placement = FlyoutPlacementMode.Top;
                flyoutBase.ShowAt(PagesControl);
            }));
        }

        void TextBlock_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            TextBlock textBlock = sender as TextBlock;
        }
    }
}
