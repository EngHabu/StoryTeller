using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace StoryTeller.ViewModel
{
    public class StorylineBuilder
    {
        internal ObservableCollection<DataModel.Model.IScene> GetScenes(StoryLineViewModel storylineModel)
        {
            ObservableCollection<IScene> scenes = new ObservableCollection<IScene>();
            foreach (SceneViewModel sceneModel in storylineModel)
            {
                scenes.Add(sceneModel.CurrentScene);
            }
            return scenes;
        }
    }
}
