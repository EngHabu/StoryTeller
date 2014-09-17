using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.ViewModel
{
    public sealed class StoryRendererViewModel : INotifyPropertyChanged
    {
        private Story _story;
        private ObservableCollection<SceneViewModel> _scenes;

        public Story Story
        {
            get { return _story; }
            set
            {
                _story = value;
                OnPropertyChanged("Story");
            }
        }

        public ObservableCollection<SceneViewModel> Scenes
        {
            get { return _scenes; }
            set
            {
                _scenes = value;
                OnPropertyChanged("Scenes");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public StoryRendererViewModel(Story story)
        {
            Story = story;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
