using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.DataModel.Model
{
    public sealed class InteractiveScene : Scene
    {
        private IList<IScene> _possibleScenes = new List<IScene>();
        public InteractiveScene(LibraryItem libraryItem) : base(libraryItem) { }

        public IList<IScene> PossibleScenes { get { return _possibleScenes; } }
    }
}
