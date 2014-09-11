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

        private StoryBuilder _builder = new StoryBuilder();

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
            Scenes = _builder.StorylineBuilder.GetScenes(CurrentStoryline);
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
                StoryLines = _builder.ConstructStoryLines(this);
                CurrentStoryline = StoryLines.First();
            }
        }

        public StoryLineViewModel CurrentStoryline { get; set; }

        public ObservableCollection<StoryLineViewModel> StoryLines { get; set; }

        internal void AddScene(IScene scene)
        {
            if (CurrentStoryline == null) {
                return;
            }
            if (CurrentStoryline.Count > 0 && CurrentStoryline.Last().CurrentScene is InteractiveScene) {
                return;
            }
            CurrentStoryline.Add(new SceneViewModel(scene));
        }

        internal void SelectStoryline(StoryLineViewModel storyline)
        {
            CurrentStoryline = storyline;
            Scenes = _builder.StorylineBuilder.GetScenes(storyline);
            if (Scenes.Count > 0)
            {
                CurrentScene = Scenes[0];
            }
        }

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
                    interactiveScene = new InteractiveScene(lastScene.CurrentScene.LibraryItem);
                    storyline.Remove(lastScene);
                    storyline.Add(new SceneViewModel(interactiveScene));
                }

                int index = FindStoryLineIndex(storyline);
                if (index > -1) {
                    IStorylinePositioner positioner = new StorylineInserter(StoryLines, index);
                    _builder.ConstructStoryLines(this, storyline, null, storyline.Count, positioner);
                }
            }
        }

        private int FindStoryLineIndex(StoryLineViewModel storyline)
        {
            for (int i = 0; i < StoryLines.Count; i++ )
            {
                if (StoryLines[i] == storyline)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
