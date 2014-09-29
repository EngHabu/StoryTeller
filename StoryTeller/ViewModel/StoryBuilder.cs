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
        private SceneViewModel m_removedItem;

        public StorylineBuilder StorylineBuilder { get { return _storylineBuilder; } }

        public ObservableCollection<StoryLineViewModel> ConstructStoryLines(StoryViewModel storyModel)
        {
            storyModel.StoryLines.Clear();
            if (storyModel.Story.StartScene != null)
            {
                ConstructStoryLines(storyModel, null, storyModel.Story.StartScene, 0, new StorylineAdder(storyModel.StoryLines));
            }
            return storyModel.StoryLines;
        }

        public void ConstructStoryLines(StoryViewModel storyModel, StoryLineViewModel parent, IScene startScene, int depth, IStorylinePositioner storylineAdder)
        {            
            IScene currentScene = startScene;

            StoryLineViewModel lineScenes = new StoryLineViewModel(storyModel, parent);
            lineScenes.Depth = depth;            

            storylineAdder.Position(lineScenes);

            int padding = depth;
            while (currentScene != null)
            {
                padding++;
                lineScenes.Add(new SceneViewModel(currentScene));
                InteractiveScene interactiveScene = currentScene as InteractiveScene;
                if (null != interactiveScene && interactiveScene.Type == SceneType.Interactive)
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
            Story story = storyModel.Story;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                SceneViewModel removedSceneModel = e.OldItems[0] as SceneViewModel;
                m_removedItem = removedSceneModel;
                InteractiveScene removedScene = removedSceneModel.CurrentScene as InteractiveScene;  
                IList<IScene> oldSuccessors = removedScene.PossibleScenes;

                if (removedScene == story.StartScene && oldSuccessors.Count == 1)
                {
                    story.StartScene = oldSuccessors[0];
                }
                else
                {
                    InteractiveScene oldPredecessor = GetPredecessor(e.OldStartingIndex, storylineModel);

                    oldPredecessor.PossibleScenes.Remove(removedScene);
                    foreach(IScene scene in oldSuccessors)
                    {
                        oldPredecessor.PossibleScenes.Add(scene);
                    }
                }
                
            }

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                SceneViewModel addedSceneModel = e.NewItems[0] as SceneViewModel;
                InteractiveScene addedScene = addedSceneModel.CurrentScene as InteractiveScene;                

                if (story.StartScene == null)
                {
                    story.StartScene = addedScene;
                    return;
                }

                int addedSceneIndex = e.NewStartingIndex;

                InteractiveScene newPredecessor = GetPredecessor(addedSceneIndex, storylineModel);
                InteractiveScene newSuccessor = GetSuccessor(addedSceneIndex, storylineModel);                
                if (newPredecessor != null)
                {
                    newPredecessor.PossibleScenes.Remove(newSuccessor);
                    newPredecessor.PossibleScenes.Add(addedScene);                                        
                }
                else
                {
                    if (newSuccessor == story.StartScene)
                    {
                        story.StartScene = addedScene;
                    }
                }

                if (newSuccessor != null)
                {
                    addedScene.PossibleScenes.Clear();
                    addedScene.PossibleScenes.Add(newSuccessor);
                }
                else {
                    addedScene.PossibleScenes.Clear();
                }

                if (m_removedItem != null && m_removedItem == addedSceneModel)
                {
                    m_removedItem = null;
                    HandleReorder(storyModel);
                }
                
            }
        }

        private void HandleReorder(StoryViewModel storyModel)
        {
            storyModel.Story = storyModel.Story;
        }

        private InteractiveScene GetSuccessor(int addedSceneIndex, StoryLineViewModel storylineModel)
        {
            int successorIndex = addedSceneIndex + 1;
            if (successorIndex < storylineModel.Count)
            {
                SceneViewModel successorSceneModel = storylineModel[successorIndex];
                return successorSceneModel.CurrentScene as InteractiveScene;
            }

            return null;
        }

        private static InteractiveScene GetPredecessor(int index, StoryLineViewModel storylineModel)
        {
            if ((index == 0 && storylineModel.Parent == null) || index > storylineModel.Count)
            {
                return null;
            }
            
            InteractiveScene oldPredecessor = null;

            if (index == 0 && storylineModel.Parent != null)
            {
                StoryLineViewModel parentStoryline = storylineModel.Parent;
                SceneViewModel parentSceneModel = parentStoryline.Last();
                if (parentSceneModel.CurrentScene is InteractiveScene)
                {
                    oldPredecessor = parentSceneModel.CurrentScene as InteractiveScene;
                }
            }
            else{
                SceneViewModel lastExistingSceneModel = storylineModel[index - 1];
                oldPredecessor = lastExistingSceneModel.CurrentScene as InteractiveScene;
            }

            return oldPredecessor;
        }


        internal StoryLineViewModel CreateStoryLine(StoryViewModel storyViewModel)
        {
            StoryLineViewModel storylineModel = new StoryLineViewModel(storyViewModel, null);
            storylineModel.CollectionChanged += lineScenes_CollectionChanged;
            return storylineModel;
        }

    }
}
