using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.DataModel.Model
{
    public sealed class LibraryItem : ILibraryItem
    {
        public string Id { get; set; }
        public ISceneContent SceneContent { get; set; }
    }
}
