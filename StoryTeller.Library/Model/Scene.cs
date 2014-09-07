using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.DataModel.Model
{
    public class Scene : IScene
    {
        private LibraryItem _libraryItem;
        private string _libraryItemId;

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

        public virtual string Id
        {
            get;
            set;
            }

        public virtual SceneType Type
        {
            get
            {
                return SceneType.Regular;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

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

        public IScene FollowingScene
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
