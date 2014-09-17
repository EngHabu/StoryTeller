using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.ViewModel
{
    public class ProjectViewModel : INotifyPropertyChanged
    {
        private StoryViewModel _story;
        private LibraryViewModel _library;
        private StoryRendererViewModel _storyRendererViewModel;

        public StoryRendererViewModel StoryRendererViewModel
        {
            get { return _storyRendererViewModel; }
            set
            {
                _storyRendererViewModel = value;
                OnPropertyChanged("StoryRendererViewModel");
            }
        }
        
        public StoryViewModel Story
        {
            get { return _story; }
            set
            {
                _story = value;
                OnPropertyChanged("Story");
            }
        }

        public LibraryViewModel Library
        {
            get { return _library; }
            set
            {
                _library = value;
                OnPropertyChanged("Library");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string changedProperty)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(changedProperty));
            }
        }
    }
}
