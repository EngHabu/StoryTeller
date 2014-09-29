using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.DataModel.Model
{
    [DataContract]
    public sealed class Story
    {
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public IScene StartScene { get; set; }

        [DataMember]
        public IEnumerable<IScene> BonusScenes { get; set; }
    }
}
