using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Windows.UI.Xaml;

namespace StoryTeller.ViewModel
{
    public class StoryLineViewModel : ObservableCollection<SceneViewModel>, INotifyPropertyChanged
    {
        private StoryViewModel storyViewModel;
        private StoryLineViewModel _parent;

        public StoryLineViewModel(StoryViewModel parent, StoryLineViewModel parentStoryline)
        {
            this.storyViewModel = parent;
            _parent = parentStoryline;
        }

        public StoryViewModel StoryModel { get { return storyViewModel; } }

        public StoryLineViewModel Parent { get { return _parent; } }

        public int Depth { get; set; }

        public int DepthWidth
        {
            get
            {
                return Depth * 200;
            }
        }

        public bool IsCurrent
        {            
            get
            {
                return StoryModel.CurrentStoryline == this;
            }
            set
            {
                OnPropertyChanged("IsCurrent");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyChanged)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyChanged));
            }
        }
    }
}
