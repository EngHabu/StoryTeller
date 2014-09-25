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
    public sealed partial class ScenePickerControl : UserControl
    {

        public delegate void ScenePickerRequest(IScene scene);
        public event ScenePickerRequest PickSceneRequest;

        public ScenePickerControl()
        {
            this.InitializeComponent();
            DataContextChanged += ScenePickerControl_DataContextChanged;
        }

        void ScenePickerControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Popup popup = FindParentObjectOfType<Popup>(this);
            if (null != popup)
            {
                popup.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private T1 FindParentObjectOfType<T1>(FrameworkElement element)
        {
            T1 result = default(T1);
            while (null != element && !(element is T1))
            {
                element = element.Parent as FrameworkElement;
            }

            return result;
        }

        private void Scenes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != PickSceneRequest)
            {
                PickSceneRequest(Scenes.SelectedItem as IScene);
            }
        }
    }
}
