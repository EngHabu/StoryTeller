using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTeller.ViewModel
{
    public interface IStorylinePositioner
    {
        void Position(StoryLineViewModel lineScenes);
    }
}
