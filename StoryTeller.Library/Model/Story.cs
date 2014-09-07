using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.DataModel.Model
{
    public sealed class Story
    {
        public string Title { get; set; }
        public IScene StartScene { get; set; }
        public IEnumerable<IScene> BonusScenes { get; set; }
    }
}
