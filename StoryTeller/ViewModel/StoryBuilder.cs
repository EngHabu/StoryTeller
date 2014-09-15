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
                padding++;
                lineScenes.Add(new SceneViewModel(currentScene));
                InteractiveScene interactiveScene = currentScene as InteractiveScene;
                if (null != interactiveScene && interactiveScene.PossibleScenes.Count > 1)
                {
                    foreach (IScene possibleStartScene in interactiveScene.PossibleScenes)
                    {
                        ConstructStoryLines(storyModel, lineScenes, possibleStartScene, padding, storylineAdder);
                    }

                    break;
                }
                else
                {
                    currentScene = currentScene.FollowingScene;
                }
            }

            lineScenes.CollectionChanged += lineScenes_CollectionChanged;
        }


        public void lineScenes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            StoryLineViewModel storylineModel = sender as StoryLineViewModel;
            if (storylineModel == null)
            {
                return;
            }

            StoryViewModel storyModel = storylineModel.StoryModel;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                if (storylineModel.Parent == null && storylineModel.Count == 0)
                {
                    storyModel.Story.StartScene = null;
                    return;
                }

                if (storylineModel.Count > 0)
                {
                    SceneViewModel lastExistingSceneModel = storylineModel.Last();
                    IScene lastExistingScene = lastExistingSceneModel.CurrentScene;
                    SceneViewModel removedScene = e.OldItems[0] as SceneViewModel;

                    if (lastExistingSceneModel is SceneViewModelPad)
                    {
                        StoryLineViewModel parentStoryline = storylineModel.Parent;
                        SceneViewModel parentSceneModel = parentStoryline.Last();
                        if (parentSceneModel.CurrentScene is InteractiveScene)
                        {
                            (parentSceneModel.CurrentScene as InteractiveScene).PossibleScenes.Remove(removedScene.CurrentScene);
                        }
                    }
                    else
                    {
                        lastExistingScene.FollowingScene = null;
                    }
                }
            }

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                SceneViewModel addedScene = storylineModel.Last();
                if (storyModel.Story.StartScene == null)
                {
                    storyModel.Story.StartScene = addedScene.CurrentScene;
                    return;
                }

                SceneViewModel previousScene = null;
                if (storylineModel.Count > 1)
                {
                    previousScene = storylineModel[storylineModel.Count - 2];
                }

                if (previousScene is SceneViewModelPad)
                {
                    StoryLineViewModel parentStoryline = storylineModel.Parent;
                    if (parentStoryline != null)
                    {
                        IScene lastParentScene = parentStoryline.Last().CurrentScene;
                        if (lastParentScene is InteractiveScene)
                        {
                            IList<IScene> possibleScenes = (lastParentScene as InteractiveScene).PossibleScenes;

                            if (possibleScenes.Count > 0)
                            {
                                possibleScenes.Insert(0, addedScene.CurrentScene);
                            }
                            else
                            {
                                possibleScenes.Add(addedScene.CurrentScene);
                            }
                        }
                    }
                }
                else if (previousScene != null)
                {
                    IScene currentScene = previousScene.CurrentScene;
                    currentScene.FollowingScene = addedScene.CurrentScene;
                }
            }
        }

    }
}
