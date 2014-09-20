using StoryTeller.Common;
using StoryTeller.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Windows;
using StoryTeller.DataModel.Model;
using Windows.Storage.Pickers;
using Windows.Storage;
using StoryTeller.Controls;
using StoryTeller.ViewModel;
using StoryTeller.Pages;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace StoryTeller
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private LibraryViewModel libraryViewModel = new LibraryViewModel();
        private ProjectViewModel projectViewModel = new ProjectViewModel();

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public MainPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            libraryViewModel.Library = new Library();
            projectViewModel.Library = libraryViewModel;

            Story story = new Story();
            StoryViewModel storyModel = new StoryViewModel(story);
            projectViewModel.Story = storyModel;
            projectViewModel.StoryRendererViewModel = new StoryRendererViewModel(story);

            this.DataContext = projectViewModel;

            libraryPanel.DoubleTapped += libraryPanel_DoubleTapped;
        }

        void storylinePanel_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
        }

        private void libraryPanel_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            LibraryItem libraryItem = libraryPanel.SelectedItem as LibraryItem;
            IScene scene = new InteractiveScene(libraryItem);
            projectViewModel.Story.AddScene(scene);
        }


        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var sampleDataGroups = await SampleDataSource.GetGroupsAsync();
            this.DefaultViewModel["Groups"] = sampleDataGroups;
        }

        /// <summary>
        /// Invoked when a group header is clicked.
        /// </summary>
        /// <param name="sender">The Button used as a group header for the selected group.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // Determine what group the Button instance represents
            var group = (sender as FrameworkElement).DataContext;

            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            this.Frame.Navigate(typeof(MainPage), ((SampleDataGroup)group).UniqueId);
        }




        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {


        }

        async private void Default_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await LoadDefaultFiles();
        }
        async private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            await LoadFilesFromStorage();
        }
        private async System.Threading.Tasks.Task LoadFilesFromStorage()
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            folderPicker.FileTypeFilter.Add(".txt");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            
            
            //FileOpenPicker picker = new FileOpenPicker();
            //picker.FileTypeFilter.Add(".txt");

            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();  //await picker.PickMultipleFilesAsync();
            await LoadFiles(FilterFiles(files));
        }

        private async System.Threading.Tasks.Task LoadDefaultFiles()
        {
            StorageFolder folder = await Windows.Storage.KnownFolders.PicturesLibrary.GetFolderAsync("StoryTeller");
            if (folder != null)
            {
                IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
                IList<StorageFile> filteredFiles = FilterFiles(files);
                await LoadFiles(filteredFiles);
            }
        }

        private static IList<StorageFile> FilterFiles(IReadOnlyList<StorageFile> files)
        {
            IList<StorageFile> filteredFiles = new List<StorageFile>();
            foreach (StorageFile file in files)
            {
                if (file.FileType.EndsWith("txt"))
                {
                    filteredFiles.Add(file);
                }
            }
            return filteredFiles;
        }

        private async System.Threading.Tasks.Task LoadFiles(IList<StorageFile> files)
        {
            foreach (StorageFile file in files)
            {
                LibraryItem libraryItem = new LibraryItem();
                libraryItem.Id = file.Name;
                libraryItem.SceneContent = new TextSceneContent();
                libraryItem.SceneContent.Content = await FileIO.ReadTextAsync(file);
                libraryViewModel.Items.Add(libraryItem);
            }
        }

        private void Reload_Tapped(object sender, TappedRoutedEventArgs e)
        {
            projectViewModel.Story.Story = projectViewModel.Story.Story;
        }

        private void Clear_Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            projectViewModel.Story.Clear();
        }

        private void PreviewRendererControl_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void More_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement uiElement = (FrameworkElement)sender;
            FlyoutBase flyout = MenuFlyout.GetAttachedFlyout(uiElement);
            flyout.Placement = FlyoutPlacementMode.Bottom;
            flyout.ShowAt(uiElement);
        }

        private void FullScreenPreview_Click(object sender, RoutedEventArgs e)
        {
            string guid = Guid.NewGuid().ToString();
            ViewModelCache.Local.Put(guid, projectViewModel.Story);
            this.Frame.Navigate(typeof(StoryViewer), guid);
        }
    }
}