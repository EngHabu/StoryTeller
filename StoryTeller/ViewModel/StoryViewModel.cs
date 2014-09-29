using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StoryTeller.ViewModel
{
    public class StoryViewModel : INotifyPropertyChanged
    {
        private DataModel.Model.Story _story;
        private IScene _currentScene;
        ObservableCollection<IScene> _scenes;
        ObservableCollection<SceneViewModel> _scenesViewModel = new ObservableCollection<SceneViewModel>();
        private double _pageWidth = 750;
        private double _pageHeight = 700;
        private StoryBuilder _builder = new StoryBuilder();
        private StoryLineViewModel _currentStoryline;
        private ObservableCollection<StoryLineViewModel> _storyLines;
        private ObservableCollection<SceneTag> _favorites;
        private SceneViewModel _selectedSceneViewModel = null;

        public event ScenePickerRequest PossibleScenePickRequest;

        public SceneViewModel SelectedSceneViewModel
        {
            get { return _selectedSceneViewModel; }
            set
            {
                _selectedSceneViewModel = value;
                OnPropertyChanged("SelectedSceneViewModel");
            }
        }

        public double PageHeight
        {
            get { return _pageHeight; }
            set { _pageHeight = value; }
        }

        public ObservableCollection<SceneTag> FavoriteTags
        {
            get
            {
                if (_favorites == null)
                {
                    _favorites = new ObservableCollection<SceneTag>();
                    _favorites.CollectionChanged += _favorites_CollectionChanged;
                }
                return _favorites;
            }
        }

        public string Title
        {
            get
            {
                return _story.Title;
            }
            set
            {
                _story.Title = value;
                OnPropertyChanged("Title");
            }
        }

        void _favorites_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RefreshScenes(CurrentStoryline);
        }

        public double PageWidth
        {
            get { return _pageWidth; }
            set
            {
                _pageWidth = value;
                OnPropertyChanged("PageWidth");
            }
        }

        public IScene CurrentScene
        {
            get { return _currentScene; }
            set
            {
                _currentScene = value;
                OnPropertyChanged("CurrentScene");
            }
        }

        public ObservableCollection<SceneViewModel> ScenesViewModel
        {
            get { return _scenesViewModel; }
            set
            {
                _scenesViewModel = value;
                OnPropertyChanged("ScenesViewModel");
            }
        }

        public ObservableCollection<IScene> Scenes
        {
            get { return _scenes; }
            set
            {
                _scenes = value;
                OnPropertyChanged("Scenes");
            }
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
                CurrentStoryline = StoryLines.FirstOrDefault();
            }
        }

        public StoryLineViewModel CurrentStoryline
        {
            get { return _currentStoryline; }
            set
            {
                StoryLineViewModel oldValue = _currentStoryline;
                _currentStoryline = value;
                UpdateIsCurrent(oldValue);
                UpdateIsCurrent(_currentStoryline);
                OnPropertyChanged("CurrentStoryline");
            }
        }

        private void UpdateIsCurrent(StoryLineViewModel storyline)
        {
            if (storyline != null)
            {
                //This value is dummy, just to trigger property notification
                storyline.IsCurrent = true;
            }
        }

        public ObservableCollection<StoryLineViewModel> StoryLines
        {
            get 
            {
                if (_storyLines == null)
                {
                    _storyLines = new ObservableCollection<StoryLineViewModel>();
                }
                return _storyLines; 
            }
            set
            {
                _storyLines = value;
                OnPropertyChanged("StoryLines");
            }
        }

        public StoryViewModel(Story story)
        {
            Story = story;
            CurrentScene = Story.StartScene;
            Scenes = _builder.StorylineBuilder.GetScenes(CurrentStoryline);
        }

        internal void Clear()
        {
            _story.StartScene = null;
            StoryLines = _builder.ConstructStoryLines(this);
            CurrentStoryline = StoryLines.FirstOrDefault();
        }

        internal void AddScene(IScene scene)
        {
            if (CurrentStoryline == null)
            {
                if (StoryLines.Count > 0)
                {
                    return;
                }
                StoryLines.Add(_builder.CreateStoryLine(this));
                CurrentStoryline = StoryLines.First();
            }

            InteractiveScene interactiveScene;
            if (CurrentStoryline.Count > 0
                && (null != (interactiveScene = CurrentStoryline.Last().CurrentScene as InteractiveScene))
                && interactiveScene.Type == SceneType.Interactive)
            {
                return;
            }

            SceneViewModel sceneViewModel = CreateSceneViewModel(scene);
            CurrentStoryline.Add(sceneViewModel);
            RefreshScenes(CurrentStoryline);
            //Scenes.Add(scene);
            //ScenesViewModel.Add(sceneViewModel);
            //Scenes = _builder.StorylineBuilder.GetScenes(CurrentStoryline);
            OnPropertyChanged("Scenes");
        }

        private void RefreshScenes(StoryLineViewModel storyline)
        {
            ScenesViewModel = new ObservableCollection<SceneViewModel>(BuildPath(storyline));
            SceneViewModel lastSceneViewModel = null;
            Func<SceneViewModel, bool> isNotPad = (sceneViewModel) => !(sceneViewModel is SceneViewModelPad);
            lastSceneViewModel = storyline.LastOrDefault(isNotPad);

            if (null != lastSceneViewModel)
            {
                Scenes = new ObservableCollection<IScene>(BuildPath(Story.StartScene, lastSceneViewModel.CurrentScene));
            }

            if (Scenes.Count > 0)
            {
                CurrentScene = Scenes[0];
                SelectedSceneViewModel = storyline.FirstOrDefault(isNotPad);
                SelectedSceneViewModel = null;
            }
        }

        private SceneViewModel CreateSceneViewModel(IScene scene)
        {
            SceneViewModel sceneViewModel = new SceneViewModel(scene);
            sceneViewModel.NavigateRequest += sceneViewModel_NavigateRequest;
            sceneViewModel.PickSceneRequest += sceneViewModel_PickSceneRequest;
            sceneViewModel.BonusSceneChanged += sceneViewModel_BonusSceneChanged;
            sceneViewModel.SceneTagsChanged += sceneViewModel_SceneTagsChanged;
            return sceneViewModel;
        }

        void sceneViewModel_SceneTagsChanged(SceneViewModel sender)
        {
            OnPropertyChanged("Tags");
        }

        void sceneViewModel_BonusSceneChanged()
        {
            RefreshScenes(CurrentStoryline);
        }

        void sceneViewModel_PickSceneRequest(object sender, ScenePickerRequestArgs args)
        {
            OnPossibleScenePickRequest(args);
        }

        void sceneViewModel_NavigateRequest(object sender, IScene scene)
        {
            foreach (StoryLineViewModel storyLineViewModel in StoryLines)
            {
                SceneViewModel firstSceneViewModel = null;
                if ((firstSceneViewModel = storyLineViewModel.FirstOrDefault(
                    (sceneViewModel) => !(sceneViewModel is SceneViewModelPad))) != null)
                {
                    if (firstSceneViewModel.CurrentScene.Id == scene.Id)
                    {
                        SelectStoryline(storyLineViewModel);
                        SelectedSceneViewModel = firstSceneViewModel;
                        break;
                    }
                }
            }
        }

        internal void SelectStoryline(StoryLineViewModel storyline)
        {
            CurrentStoryline = storyline;
            RefreshScenes(storyline);
        }

        private IEnumerable<SceneViewModel> BuildPath(StoryLineViewModel targetStoryLine)
        {
            List<SceneViewModel> result = new List<SceneViewModel>();
            result.AddRange(ReverseAndFilter(targetStoryLine));
            StoryLineViewModel parent = targetStoryLine;
            while (null != (parent = parent.Parent))
            {
                result.AddRange(ReverseAndFilter(parent));
            }

            result.Reverse();
            return result;
        }

        private static IEnumerable<SceneViewModel> ReverseAndFilter(StoryLineViewModel targetStoryLine)
        {
            return (from scene in targetStoryLine
                    where (!scene.IsBonus || (scene.IsBonus && isFavorited(targetStoryLine, scene)))
                    select scene).Reverse();
        }

        private static bool isFavorited(StoryLineViewModel targetStoryLine, SceneViewModel scene)
        {
            ObservableCollection<SceneTag> favoriteTags = targetStoryLine.StoryModel.FavoriteTags;
            if (favoriteTags.Count == 0)
            {
                return false;
            }
            foreach (SceneTag sceneTag in scene.Tags)
            {
                foreach (SceneTag favoriteTag in favoriteTags)
                {
                    if (favoriteTag.Content.Equals(sceneTag.Content))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        private IEnumerable<IScene> BuildPath(IScene start, IScene targetScene)
        {
            List<IScene> result = new List<IScene>();
            if (null == start)
            {
                return result;
            }

            if (start.Id == targetScene.Id)
            {
                result.Add(targetScene);
                return result;
            }

            InteractiveScene interactiveScene = start as InteractiveScene;
            foreach (IScene scene in interactiveScene.PossibleScenes)
            {
                IEnumerable<IScene> route = BuildPath(scene, targetScene);
                if (route.FirstOrDefault() != null)
                {
                    result.Add(start);
                    result.AddRange(route);
                    break;
                }
            }

            return result;
        }

        internal void AddStoryline(StoryLineViewModel storyline)
        {
            if (storyline.Count > 0)
            {
                SceneViewModel lastScene = storyline.Last();

                InteractiveScene interactiveScene;
                if (lastScene.CurrentScene is InteractiveScene)
                {
                    interactiveScene = lastScene.CurrentScene as InteractiveScene;
                    if (null != interactiveScene)
                    {
                        interactiveScene.Type = SceneType.Interactive;
                    }
                }
                else
                {
                    interactiveScene = new InteractiveScene(lastScene.CurrentScene.LibraryItem);
                    storyline.Remove(lastScene);
                    storyline.Add(CreateSceneViewModel(interactiveScene));
                }

                int index = FindStoryLineIndex(storyline);
                if (index > -1)
                {
                    IStorylinePositioner positioner = new StorylineInserter(StoryLines, index);
                    _builder.ConstructStoryLines(this, storyline, null, storyline.Depth + storyline.Count, positioner);
                    SelectStoryline(StoryLines[index + 1]);
                }
            }
        }

        private int FindStoryLineIndex(StoryLineViewModel storyline)
        {
            for (int i = 0; i < StoryLines.Count; i++)
            {
                if (StoryLines[i] == storyline)
                {
                    return i;
                }
            }
            return -1;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyChanged)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyChanged));
            }
        }

        private void OnPossibleScenePickRequest(ScenePickerRequestArgs args)
        {
            if (null != PossibleScenePickRequest)
            {
                args.SenderChain.Add(this);
                PossibleScenePickRequest(this, args);
            }
        }

        public bool IsEmpty
        {
            get
            {
                return Story.StartScene == null;
            }
        }
    }
}
