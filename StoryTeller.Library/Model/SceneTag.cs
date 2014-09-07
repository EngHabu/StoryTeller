using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTeller.DataModel.Model
{
    public sealed class SceneTag
    {
        string Id { get; set; }
        string Name { get; set; }
        string Content { get; set; }
        SceneTagType Type { get; set; }
    }
}
