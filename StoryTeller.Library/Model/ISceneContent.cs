using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTeller.Library.Model
{
    public interface ISceneContent
    {
        public SceneContentType Type { get; set; }
        public object Content { get; set; }
    }
}
