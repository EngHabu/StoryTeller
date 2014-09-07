using System;
namespace StoryTeller.DataModel.Model
{
    public interface ILibraryItem
    {
        string Id { get; set; }
        ISceneContent SceneContent { get; set; }
    }
}
