using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.Library.Model
{
    public interface IScene
    {
        string Id { get; set; }
        public SceneType Type { get; set; }
        ISceneContent Content { get; set; }
        IScene FollowingScene { get; set; }
    }
}
