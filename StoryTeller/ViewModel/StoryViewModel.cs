using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.ViewModel
{
    public sealed class StoryViewModel
    {
        private Story _story;
        private IScene _currentScene;
        ObservableCollection<IScene> _scenes;

        public IScene CurrentScene
        {
            get { return _currentScene; }
            set { _currentScene = value; }
        }

        public Story Story
        {
            get { return _story; }
            set { _story = value; }
        }

        public ObservableCollection<IScene> Scenes
        {
            get { return _scenes; }
            set { _scenes = value; }
        }

        public StoryViewModel(Story story)
        {
            Story = story;
            CurrentScene = Story.StartScene;
            Scenes = ComputeFirstStory(Story);
        }

        private ObservableCollection<IScene> ComputeFirstStory(Story Story)
        {
            List<IScene> scenes = new List<IScene>();
            for (IScene current = Story.StartScene; current != null; current = current.FollowingScene)
            {
                scenes.Add(current);
            }

            return new ObservableCollection<IScene>(scenes);
        }
    }
}
