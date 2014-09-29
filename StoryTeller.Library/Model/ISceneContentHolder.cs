using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.DataModel.Model
{
    public interface ISceneContentHolder : INotifyPropertyChanged
    {
        IList<SceneTag> Tags { get; }
        SceneContentType Type { get; set; }
        string Content { get; set; }
    }
}
