using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.DataModel.Model
{
    public class TempSceneContent : ISceneContent
    {

        public SceneContentType Type
        {
            get;
            set;
        }

        public object Content
        {
            get;
            set;
        }

        public IEnumerable<SceneTag> Tags
        {
            get;
            set;
        }
    }
}
