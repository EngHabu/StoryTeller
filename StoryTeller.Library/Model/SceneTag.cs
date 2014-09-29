using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace StoryTeller.DataModel.Model
{
    [DataContract]
    public sealed class SceneTag
    {
        public SceneTag(string name, string content)
        {
            Type = SceneTagType.Custom;
            Name = name;
            Content = content;
        }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public SceneTagType Type { get; set; }
    }
}
