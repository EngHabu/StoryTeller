using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.ViewModel
{
    public class ProjectViewModel
    {
        public StoryViewModel Story {get; set;}
        public LibraryViewModel Library { get; set; }
    }
}
