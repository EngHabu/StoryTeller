﻿using StoryTeller.DataModel.Model;
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

        public event ScenePickerRequest PossibleScenePickRequest;

        public double PageHeight
        {
            get { return _pageHeight; }
            set { _pageHeight = value; }
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
                CurrentStoryline = StoryLines.First();
            }
        }

        public StoryLineViewModel CurrentStoryline 
        {
            get { return _currentStoryline; }
            set
            {
                _currentStoryline = value;
                OnPropertyChanged("CurrentStoryline");
            }
        }

        public ObservableCollection<StoryLineViewModel> StoryLines
        {
            get { return _storyLines; }
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
            CurrentStoryline = StoryLines.First();
        }
        
        internal void AddScene(IScene scene)
        {
            if (CurrentStoryline == null)
            {
                return;
            }

            InteractiveScene interactiveScene;
            if (CurrentStoryline.Count > 0 
                && (null != (interactiveScene = CurrentStoryline.Last().CurrentScene as InteractiveScene))
                && interactiveScene.PossibleScenes.Count > 0)
            {
                return;
            }
            
            SceneViewModel sceneViewModel = CreateSceneViewModel(scene);
            CurrentStoryline.Add(sceneViewModel);
            Scenes.Add(scene);
            ScenesViewModel.Add(sceneViewModel);
            //Scenes = _builder.StorylineBuilder.GetScenes(CurrentStoryline);
            OnPropertyChanged("Scenes");
        }

        private SceneViewModel CreateSceneViewModel(IScene scene)
        {
            SceneViewModel sceneViewModel = new SceneViewModel(scene);
            sceneViewModel.NavigateRequest += sceneViewModel_NavigateRequest;
            sceneViewModel.PickSceneRequest += sceneViewModel_PickSceneRequest;
            return sceneViewModel;
        }

        void sceneViewModel_PickSceneRequest(object sender, ScenePickerRequestArgs args)
        {
            OnPossibleScenePickRequest(args);
        }

        void sceneViewModel_NavigateRequest(object sender, IScene scene)
        {
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
                if (lastScene.CurrentScene is InteractiveScene)
                {
                    interactiveScene = lastScene.CurrentScene as InteractiveScene;
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
                    _builder.ConstructStoryLines(this, storyline, null, storyline.Count, positioner);
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
            if (null == propertyChanged)
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
    }
}
