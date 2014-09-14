using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.DataModel.Model
{
    public sealed class TextSceneContent : ISceneContent
    {
        private string _content;
        private IList<SceneTag> _tags;
        public SceneContentType Type
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

        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
            }
        }

        public IList<SceneTag> Tags
        {
            get
            {
                if (_tags == null) 
                {
                    _tags = new List<SceneTag>();
                }
                return _tags;
            }
        }
    }
}
