using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.DataModel.Model
{
    public sealed class LibraryItem : ILibraryItem, ISceneContentHolder
    {
        IList<SceneTag> _tags = new List<SceneTag>();

        public string Id { get; set; }
        public ISceneContent SceneContent { get; set; }

        public IList<SceneTag> Tags
        {
            get { return _tags; }
        }

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
            }
        }
    }
}
