using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.DataModel.Model
{
    [DataContract]
    public class Scene : IScene
    {
        private LibraryItem _libraryItem;
        private string _libraryItemId;
        private IList<SceneTag> _tags;

        public Scene()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Scene(LibraryItem libraryItem)
            : this()
        {
            _libraryItem = libraryItem;
            _libraryItemId = libraryItem.Id;
        }

        public string LibraryItemId
        {
            get { return _libraryItemId; }
            set { _libraryItemId = value; }
        }

        [DataMember]
        public IList<SceneTag> Tags
        {
            get
            {
                return Content.Tags;
            }
            set
            {
                Content.Tags = value;
            }
        }

        [IgnoreDataMember]
        public LibraryItem LibraryItem
        {
            get
            {
                if (_libraryItem == null)
                {
                    _libraryItem = Library.GetItem(_libraryItemId);
                }

                return _libraryItem;
            }
            set
            {
                _libraryItem = value;
            }
        }

        [DataMember]
        public virtual string Id
        {
            get;
            set;
        }

        [DataMember]
        public virtual SceneType Type
        {
            get;
            set;
        }

        [IgnoreDataMember]
        public ISceneContent Content
        {
            get
            {
                if (null == _libraryItem)
                {
                    _libraryItem = Library.GetItem(_libraryItemId);
                }

                if (null != _libraryItem)
                {
                    return _libraryItem.SceneContent;
                }

                return null;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        [DataMember]
        public virtual IScene FollowingScene
        {
            get;
            set;
        }


        public bool IsBonusScene
        {
            get;
            set;
        }
    }
}
