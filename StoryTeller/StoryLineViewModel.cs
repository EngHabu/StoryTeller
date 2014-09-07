using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace StoryTeller
{
    class StoryLineViewModel : ObservableCollection<SceneViewModel>
    {
        private string lineID;

        public StoryLineViewModel(string lineID)
        {
            // TODO: Complete member initialization
            this.lineID = lineID;
        }
    }
}
