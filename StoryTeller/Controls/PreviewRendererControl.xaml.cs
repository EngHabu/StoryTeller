using StoryTeller.DataModel;
using StoryTeller.DataModel.Model;
using StoryTeller.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                storyViewModel.PropertyChanged += storyViewModel_PropertyChanged;
            }
        }

        void storyViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            object currentDataContext = PagesControl.DataContext;//.GetBindingExpression(ItemsControl.ItemsSourceProperty).
            PagesControl.DataContext = null;
            PagesControl.DataContext = currentDataContext;
            ObservableCollection<SceneViewModel> scenes = (currentDataContext as StoryViewModel).ScenesViewModel;
            if(scenes.Count > 0)
            {
                tagsList.DataContext = scenes.First().Tags;
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
                        SceneViewModel sceneViewModel = args.SenderChain.Where((obj) =>
                            (obj is SceneViewModel)).FirstOrDefault() as SceneViewModel;
                        if (null != sceneViewModel)
                        {
                            InteractiveScene interactiveScene = sceneViewModel.CurrentScene as InteractiveScene;
                            if (null != interactiveScene)
                            {
                                ScenePickerViewModel scenePickerModel = ScenePickerViewModel.Create(interactiveScene);
                                scenePickerModel.SelectedScene = interactiveScene.LookupSceneByLinkId(args.LinkId);
                                scenePickerModel.LinkId = args.LinkId;
                                scenePicker.DataContext = scenePickerModel;
                            }
                        }
                    }
                }

                flyoutBase.Placement = FlyoutPlacementMode.Top;
                flyoutBase.ShowAt(this);
            }));
        }

        void TextBlock_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            TextBlock textBlock = sender as TextBlock;
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ScrollViewer scrollViewer = sender as ScrollViewer;
            foreach (FrameworkElement frameworkElement in PagesControl.Items)
            {
                if (IsUserVisible(frameworkElement, scrollViewer))
                {
                    SceneViewModel sceneModel = frameworkElement.DataContext as SceneViewModel;
                    tagsList.DataContext = sceneModel.Tags;
                    break;
                }
            }
            
            
            ItemsPresenter itemsPresenter = scrollViewer.Content as ItemsPresenter;                        
        }

        private bool IsUserVisible(FrameworkElement element, FrameworkElement container)
        {

            Rect bounds = element.TransformToVisual(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
            Rect rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
            return rect.Contains(new Point(bounds.Left, bounds.Top)) || rect.Contains(new Point(bounds.Right, bounds.Bottom));
        }

        private void tagsList_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {            
            SceneTag selectedTag = tagsList.SelectedTag;

            if (selectedTag != null)
            {
                StoryViewModel storyModel = DataContext as StoryViewModel;
                storyModel.FavoriteTags.Add(new SceneTag(selectedTag.Name, selectedTag.Content));
            }
        }
    }
}
