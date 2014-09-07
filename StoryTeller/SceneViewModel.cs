using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTeller
{
    class SceneViewModel
    {

        public IScene CurrentScene { get; set; }

        public IScene PreviousScene { get; set; }

        public IScene NextScene { get; set; }

        public SceneViewModel(IScene currentScene)
        {
            // TODO: Complete member initialization
            this.CurrentScene = currentScene;
        }
    }
}
