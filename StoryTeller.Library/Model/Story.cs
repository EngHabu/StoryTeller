using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.Library.Model
{
    public sealed class Story
    {
        public IScene StartScene { get; set; }
        public IEnumerable<IScene> BonusScenes { get; set; }
    }
}
