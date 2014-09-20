using StoryTeller.DataModel;
using StoryTeller.DataModel.Model;
using StoryTeller.Pages;
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
        
        public PreviewRendererControl()
        {
            this.InitializeComponent();
            DataContextChanged += PreviewRendererControl_DataContextChanged;
        }

        private static bool AreEqual(double pageWidth1, double pageWidth2)
        {
            return Math.Abs(pageWidth1 - pageWidth2) < _pageWidthDiffEpsilon;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            _pageWidth = availableSize.Width / ColumnsCount;
            StoryViewModel model = DataContext as StoryViewModel;
            if (null != model && !AreEqual(_pageWidth, model.PageWidth))
            {
                model.PageWidth = _pageWidth;
                model.PageHeight = _pageHeight;
                RebindView();
            }

            return base.MeasureOverride(availableSize);
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
            RebindView();
        }

        private void RebindView()
        {
            object currentDataContext = PagesControl.DataContext;//.GetBindingExpression(ItemsControl.ItemsSourceProperty).
            PagesControl.DataContext = null;
            PagesControl.DataContext = currentDataContext;
            SceneViewModel sceneViewModel = (currentDataContext as StoryViewModel).ScenesViewModel.FirstOrDefault();
            if (null != sceneViewModel)
            {
                tagsList.DataContext = sceneViewModel.Tags;
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

        private void fullscreen_Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string guid = Guid.NewGuid().ToString();
            ViewModelCache.Local.Put(guid, DataContext as StoryViewModel);
            
            (Window.Current.Content as Frame).Navigate(typeof(StoryViewer), guid);
        }
    }
}
