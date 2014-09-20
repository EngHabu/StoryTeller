using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTeller.ViewModel
{
    public class DummyScene : Scene
    {
        public DummyScene()
            : base(new LibraryItem() { SceneContent = new TextSceneContent()})
        {
        }
    }
}
