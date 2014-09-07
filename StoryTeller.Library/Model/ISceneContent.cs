using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTeller.DataModel.Model
{
    public interface ISceneContent
    {
        SceneContentType Type { get; set; }
        object Content { get; set; }
        IEnumerable<SceneTag> Tags { get; set; }
    }
}
