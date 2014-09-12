using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace StoryTeller.ViewModel
{
    public class StoryLineViewModel : ObservableCollection<SceneViewModel>
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
    }
}
