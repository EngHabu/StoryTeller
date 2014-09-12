using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTeller.ViewModel
{
    public class StorylineAdder : IStorylinePositioner
    {
        private System.Collections.ObjectModel.ObservableCollection<StoryLineViewModel> storylines;

        public StorylineAdder(System.Collections.ObjectModel.ObservableCollection<StoryLineViewModel> storylines)
        {
            this.storylines = storylines;
        }

        public void Position(StoryLineViewModel lineScenes)
        {
            storylines.Add(lineScenes);
        }
    }
}
