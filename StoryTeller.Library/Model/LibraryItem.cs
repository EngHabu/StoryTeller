using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.DataModel.Model
{
    [DataContract]
    public sealed class LibraryItem : ILibraryItem, ISceneContentHolder, INotifyPropertyChanged
    {
        IList<SceneTag> _tags = new List<SceneTag>();

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public ISceneContent SceneContent { get; set; }

        [DataMember]
        public IList<SceneTag> Tags
        {
            get { return _tags; }
            set { _tags = value; }
        }

        [DataMember]
        public SceneContentType Type
        {
            get
            {
                return SceneContent.Type;
            }
            set
            {
                SceneContent.Type = value;
            }
        }

        public string Content
        {
            get
            {
                return SceneContent.Content;
            }
            set
            {
                SceneContent.Content = value;
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
