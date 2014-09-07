using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTeller.Library.Model
{
    public interface ISceneContent
    {
        SceneContentType Type { get; set; }
        object Content { get; set; }
        IEnumerable<SceneTag> Tags { get; set; }
    }
}
