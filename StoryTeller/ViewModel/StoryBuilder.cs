using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace StoryTeller.ViewModel
{
    public class StoryBuilder
    {
        private StorylineBuilder _storylineBuilder = new StorylineBuilder();

        public StorylineBuilder StorylineBuilder { get { return _storylineBuilder; } }

        public ObservableCollection<StoryLineViewModel> ConstructStoryLines(StoryViewModel storyModel)
        {
            if (storyModel.StoryLines == null)
            {
                storyModel.StoryLines = new ObservableCollection<StoryLineViewModel>();
            }

            storyModel.StoryLines.Clear();
            ConstructStoryLines(storyModel, null, storyModel.Story.StartScene, 0, new StorylineAdder(storyModel.StoryLines));
            return storyModel.StoryLines;
        }

        public void ConstructStoryLines(StoryViewModel storyModel, StoryLineViewModel parent, IScene startScene, int depth, IStorylinePositioner storylineAdder)
        {
            IScene currentScene = startScene;

            StoryLineViewModel lineScenes = new StoryLineViewModel(storyModel, parent);

            for (int i = 0; i < depth; i++)
            {
                lineScenes.Add(new SceneViewModelPad());
            }

            storylineAdder.Position(lineScenes);           

            int padding = depth;
            while (currentScene != null)
            {
                lineScenes.Add(new SceneViewModel(currentScene));
                if (currentScene is InteractiveScene)
                {
                    InteractiveScene interactiveScene = currentScene as InteractiveScene;                    
                    foreach (IScene possibleStartScene in interactiveScene.PossibleScenes)
                    {
                        ConstructStoryLines(storyModel, lineScenes, possibleStartScene, padding, storylineAdder);
                    }
                    break;
                }
                else
                {
                    padding++;
                    currentScene = currentScene.FollowingScene;
                }
            }

            lineScenes.CollectionChanged += lineScenes_CollectionChanged;
        }


        public void lineScenes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            StoryLineViewModel storylineModel = sender as StoryLineViewModel;
            if (storylineModel == null) {
                return;
            }

            StoryViewModel storyModel = storylineModel.StoryModel;
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (storyModel.Story.StartScene == null)
                {
                    storyModel.Story.StartScene = (e.NewItems[0] as SceneViewModel).CurrentScene;
                    return;
                }
                SceneViewModel lastSceneModel = storylineModel.Last();
                IScene newScene = (e.NewItems[0] as SceneViewModel).CurrentScene;
                if (lastSceneModel is SceneViewModelPad)
                {
                    StoryLineViewModel parentStoryline = storylineModel.Parent;
                    if (parentStoryline != null)
                    {
                        IScene lastParentScene = parentStoryline.Last().CurrentScene;
                        if (lastParentScene is InteractiveScene)
                        {
                            (lastParentScene as InteractiveScene).PossibleScenes.Add(newScene);
                        }
                    }
                }
                else {
                    IScene currentScene = lastSceneModel.CurrentScene;
                    currentScene.FollowingScene = newScene;
                }                
            }            
        }

    }
}
