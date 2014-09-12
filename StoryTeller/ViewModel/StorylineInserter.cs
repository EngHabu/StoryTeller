using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTeller.ViewModel
{
    public class StorylineInserter : IStorylinePositioner
    {
        private System.Collections.ObjectModel.ObservableCollection<StoryLineViewModel> storyLines;
        private int index;

        public StorylineInserter(System.Collections.ObjectModel.ObservableCollection<StoryLineViewModel> StoryLines, int index)
        {
            this.storyLines = StoryLines;
            this.index = index;
        }

        public void Position(StoryLineViewModel lineScenes)
        {
            AddStoryline(index, lineScenes);
        }

        private StoryLineViewModel AddStoryline(int index, StoryLineViewModel childStorylineModel)
        {            
            if (index < storyLines.Count - 1)
            {
                storyLines.Insert(index + 1, childStorylineModel);
            }
            else
            {
                storyLines.Add(childStorylineModel);
            }
            return childStorylineModel;
        }

    }
}
