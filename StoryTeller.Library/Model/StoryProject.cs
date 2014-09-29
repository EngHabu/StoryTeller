using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.DataModel.Model
{
    [DataContract]
    public sealed class StoryProject
    {
        [DataMember]
        public Story Story { get; set; }

        [DataMember]
        public Library Library { get; set; }

        public StoryProject Export(StoryProjectExportOptions options)
        {
            return this;
        }
    }
}
