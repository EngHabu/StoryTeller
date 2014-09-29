using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StoryTeller.ViewModel
{
    public delegate void SceneNavigateRequest(object sender, IScene scene);
    public delegate void ScenePickerRequest(object sender, ScenePickerRequestArgs args);
    public delegate void SceneBonusChanged();
    public delegate void SceneTagsChanged(SceneViewModel sender);

    public class SceneViewModel : ISceneContentHolder, INotifyPropertyChanged
    {
        public event SceneNavigateRequest NavigateRequest;
        public event ScenePickerRequest PickSceneRequest;
        public event SceneBonusChanged BonusSceneChanged;
        public event SceneTagsChanged SceneTagsChanged;
        private ObservableCollection<SceneTag> _tags;

        public IScene CurrentScene { get; set; }

        public ObservableCollection<SceneTag> Tags
        {
            get
            {
                return _tags;
            }
            set
            {
                _tags = value;
            }
        }

        public bool IsBonus
        {
            get
            {
                return CurrentScene.IsBonusScene;
            }
            set
            {
                CurrentScene.IsBonusScene = value;
                BonusSceneChanged();
            }
        }

        private void NotifyBonusScenesChanged()
        {
            
        }

        public SceneViewModel(IScene currentScene)
        {
            this.CurrentScene = currentScene;
            _tags = null;
            if (null != currentScene)
            {
                _tags = new ObservableCollection<SceneTag>(currentScene.Tags);
            }
            else
            {
                _tags = new ObservableCollection<SceneTag>();
            }

            _tags.CollectionChanged += _tags_CollectionChanged;
        }

        void _tags_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (SceneTag newTag in e.NewItems)
                {
                    CurrentScene.Tags.Add(newTag);
                }
                //SceneTagsChanged(this);
            }
        }

        public void LinkClicked(string linkId)
        {
            InteractiveScene interactiveScene = CurrentScene as InteractiveScene;
            if (null != interactiveScene)
            {
                IScene linkedScene = interactiveScene.LookupSceneByLinkId(linkId);
                if (null != linkedScene)
                {
                    OnNavigateRequest(linkedScene);
                }
                else if (interactiveScene.Type == SceneType.Interactive && interactiveScene.PossibleScenes.Count > 0)
                {
                    OnPickSceneRequest(linkId);
                }
            }
        }

        private void OnPickSceneRequest(string linkId)
        {
            if (null != PickSceneRequest)
            {
                ScenePickerRequestArgs args = new ScenePickerRequestArgs();
                args.LinkId = linkId;
                args.SenderChain.Add(this);
                PickSceneRequest(this, args);
            }
        }

        private void OnNavigateRequest(IScene scene)
        {
            if (null != NavigateRequest)
            {
                NavigateRequest(this, scene);
            }
        }

        internal void CreateLinkAt(Windows.UI.Xaml.Documents.TextPointer start, Windows.UI.Xaml.Documents.TextPointer end)
        {
            throw new NotImplementedException();
        }

        IList<SceneTag> ISceneContentHolder.Tags
        {
            get { return _tags; }
        }

        public SceneContentType Type
        {
            get
            {
                return CurrentScene.Content.Type;
            }
            set
            {
                CurrentScene.Content.Type = value;
            }
        }

        public string Content
        {
            get
            {
                return CurrentScene.Content.Content;
            }
            set
            {
                CurrentScene.Content.Content = value;
                OnPropertyChanged("Content");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyChanged)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyChanged));
            }
        }
    }
}
