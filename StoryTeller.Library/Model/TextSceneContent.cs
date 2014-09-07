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
        private IEnumerable<SceneTag> _tags;
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

        public IEnumerable<SceneTag> Tags
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
    }
}
