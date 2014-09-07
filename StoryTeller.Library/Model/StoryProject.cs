using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.DataModel.Model
{
    public sealed class StoryProject
    {
        public Story Story { get; set; }
        public Library Library { get; set; }

        public StoryProject Export(StoryProjectExportOptions options)
        {
            return this;
        }
    }
}
