using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTeller.DataModel.Model
{
    public sealed class SceneTag
    {

        public SceneTag(string name, string content)
        {
            Type = SceneTagType.Custom;
            Name = name;
            Content = content;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public SceneTagType Type { get; set; }
    }
}
