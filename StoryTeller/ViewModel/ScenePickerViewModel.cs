using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.ViewModel
{
    public sealed class ScenePickerViewModel : INotifyPropertyChanged
    {
        private IEnumerable<IScene> _scenes;
        private IScene _selectedScene;
        private InteractiveScene _interactiveScene;
        private string _linkId;

        public event PropertyChangedEventHandler PropertyChanged;

        public InteractiveScene InteractiveScene
        {
            get { return _interactiveScene; }
            set { _interactiveScene = value; }
        }

        public string LinkId
        {
            get { return _linkId; }
            set
            {
                _linkId = value;
                if (null != SelectedScene && !string.IsNullOrWhiteSpace(LinkId))
                {
                    InteractiveScene.LinkIdToSceneId[LinkId] = _selectedScene.Id;
                }

                OnPropertyChanged("LinkId");
            }
        }
        
        public IScene SelectedScene
        {
            get { return _selectedScene; }
            set
            {
                _selectedScene = value;
                if (null != _selectedScene && !string.IsNullOrWhiteSpace(LinkId))
                {
                    InteractiveScene.LinkIdToSceneId[LinkId] = _selectedScene.Id;
                }

                OnPropertyChanged("SelectedScene");
            }
        }

        public IEnumerable<IScene> Scenes
        {
            get { return InteractiveScene.PossibleScenes; }
        }

        private ScenePickerViewModel()
        {
        }

        public static ScenePickerViewModel Create(InteractiveScene interactiveScene)
        {
            if (null == interactiveScene)
            {
                throw new ArgumentNullException("interactiveScene");
            }

            return new ScenePickerViewModel()
            {
                InteractiveScene = interactiveScene
            };
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
