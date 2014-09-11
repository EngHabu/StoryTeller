using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.DataModel.Model
{
    public sealed class InteractiveScene : Scene
    {
        public InteractiveScene(LibraryItem libraryItem) : base(libraryItem) { }

        public IList<IScene> PossibleScenes { get; set; }
    }
}
