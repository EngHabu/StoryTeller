using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.DataModel.Model
{
    public abstract class Scene : IScene
    {
        private LibraryItem _libraryItem;
        private string _libraryItemId;

        public Scene()
        {
        }

        public Scene(LibraryItem libraryItem)
        {
            _libraryItem = libraryItem;
            _libraryItemId = libraryItem.Id;
        }

        public virtual string Id
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public virtual SceneType Type
        {
            get
            {
                throw new NotImplementedException();
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
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public bool IsBonusScene
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
