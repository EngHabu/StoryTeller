using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace StoryTeller.ViewModel
{
    public class StoryViewModel
    {
        private DataModel.Model.Story _story;
        private IScene _currentScene;
        ObservableCollection<IScene> _scenes;

        public IScene CurrentScene
        {
            get { return _currentScene; }
            set { _currentScene = value; }
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

        public Story Story
        {
            get
            {
                return _story;
            }
            set
            {
                _story = value;
                StoryLines = ConstructStoryLines();
                CurrentStoryline = StoryLines.First();
            }
        }

        public ObservableCollection<StoryLineViewModel> StoryLines { get; set; }

        private ObservableCollection<StoryLineViewModel> ConstructStoryLines()
        {
            ObservableCollection<StoryLineViewModel> storylines = new ObservableCollection<StoryLineViewModel>();
            ConstructStoryLines(Story.StartScene, "1", 0, storylines);
            return storylines;
        }

        private void ConstructStoryLines(IScene startScene, string lineID, int depth, ObservableCollection<StoryLineViewModel> storylines)
        {
            IScene currentScene = startScene;

            StoryLineViewModel lineScenes = new StoryLineViewModel(lineID, this);
            lineScenes.CollectionChanged += lineScenes_CollectionChanged;

            storylines.Add(lineScenes);

            for (int i = 0; i < depth; i++)
            {
                lineScenes.Add(new SceneViewModelPad());
            }

            int padding = depth;
            while (currentScene != null)
            {
                if (currentScene is InteractiveScene)
                {
                    InteractiveScene interactiveScene = currentScene as InteractiveScene;
                    int childID = 0;
                    foreach (IScene possibleStartScene in interactiveScene.PossibleScenes)
                    {
                        childID++;
                        ConstructStoryLines(possibleStartScene, GetLineID(lineID, childID), padding, storylines);
                    }
                }
                else
                {
                    lineScenes.Add(new SceneViewModel(currentScene));
                    padding++;
                }
            }
        }

        private static string GetLineID(string major, int minor)
        {
            return String.Format("%s.%d", major, minor);
        }

        void lineScenes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Story.StartScene == null)
            {
                Story.StartScene = (e.NewItems[0] as SceneViewModel).CurrentScene;
                return;
            }
            IScene currentScene = CurrentStoryline.First().CurrentScene;
            while (currentScene.FollowingScene != null)
            {
                currentScene = currentScene.FollowingScene;
            }
            currentScene.FollowingScene = (e.NewItems[0] as SceneViewModel).CurrentScene;
        }

        internal void AddScene(IScene scene)
        {
            CurrentStoryline.Add(new SceneViewModel(scene));
        }

        internal void SelectStoryline(StoryLineViewModel storyline)
        {
            CurrentStoryline = storyline;
            List<IScene> scenes = new List<IScene>();
            foreach (SceneViewModel sceneModel in storyline)
            {
                {
                    scenes.Add(sceneModel.CurrentScene);
                }

                if (scenes.Count > 0)
                {
                    CurrentScene = scenes[0];
                    Scenes = new ObservableCollection<IScene>(scenes);
                }
            }
        }

        public StoryLineViewModel CurrentStoryline { get; set; }

        internal void AddStoryline(StoryLineViewModel storyline)
        {
            if (storyline.Count > 0)
            {
                SceneViewModel lastScene = storyline.Last();

                InteractiveScene interactiveScene;
                if (lastScene.CurrentScene is InteractiveScene) {
                    interactiveScene = lastScene.CurrentScene as InteractiveScene;
                }
                else {
                    interactiveScene = new InteractiveScene();
                    storyline.Add(new SceneViewModel(interactiveScene));
                }                                
            }
        }
    }
}
