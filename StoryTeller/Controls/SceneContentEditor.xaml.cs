using StoryTeller.DataModel.Model;
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
    public sealed partial class SceneContentEditor : UserControl
    {

        public delegate void EditActionComplete();
        public event EditActionComplete EditComplete;

        public delegate void ReplicateRequest(ILibraryItem libraryItem);
        public event ReplicateRequest ReplicaCreated;

        public SceneContentEditor()
        {
            this.InitializeComponent();
        }

        private void okButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ISceneContentHolder sceneContainer = DataContext as ISceneContentHolder;
            sceneContainer.Content = contentText.Text;
            if (EditComplete != null)
            {
                EditComplete();
            }
        }

        private void replicateButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string selectedText = contentText.SelectedText;
            if (string.IsNullOrEmpty(selectedText))
            {
                selectedText = contentText.Text;
            }

            LibraryItem libraryItem = new LibraryItem();
            libraryItem.SceneContent = new TextSceneContent();
            libraryItem.SceneContent.Content = selectedText;
            libraryItem.Id = "replicated";

            ReplicaCreated(libraryItem);
        }
    }
}
