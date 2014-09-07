using System;
namespace StoryTeller.Library.Model
{
    public interface ILibraryItem
    {
        string Id { get; set; }
        ISceneContent SceneContent { get; set; }
    }
}
