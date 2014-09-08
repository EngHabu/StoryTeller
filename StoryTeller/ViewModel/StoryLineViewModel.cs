using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace StoryTeller.ViewModel
{
    public class StoryLineViewModel : ObservableCollection<SceneViewModel>
    {
        private string lineID;
        private StoryViewModel storyViewModel;

        public StoryLineViewModel(string lineID, StoryViewModel parent)
        {
            // TODO: Complete member initialization
            this.lineID = lineID;
            this.storyViewModel = parent;
        }

        public StoryViewModel StoryModel { get { return storyViewModel; } }
    }
}
