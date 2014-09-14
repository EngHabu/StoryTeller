using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace StoryTeller.ViewModel
{
    public class SceneViewModel
    {
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

        public SceneViewModel(IScene currentScene)
        {
            this.CurrentScene = currentScene;
            if (currentScene != null)
            {
                _tags = new ObservableCollection<SceneTag>(currentScene.Tags);
                _tags.CollectionChanged += _tags_CollectionChanged;
            }
        }

        void _tags_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) {
                foreach (SceneTag newTag in e.NewItems)
                {
                    CurrentScene.Tags.Add(newTag);
                }
            }
        }
    }
}
