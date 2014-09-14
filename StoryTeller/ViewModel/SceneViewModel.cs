using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTeller.ViewModel
{
    public delegate void SceneNavigateRequest(object sender, IScene scene);
    public delegate void ScenePickerRequest(object sender, string linkId);

    public class SceneViewModel
    {
        public event SceneNavigateRequest NavigateRequest;
        public event ScenePickerRequest PickSceneRequest;

        public IScene CurrentScene { get; set; }

        public IScene PreviousScene { get; set; }

        public IScene NextScene { get; set; }

        public SceneViewModel(IScene currentScene)
        {
            // TODO: Complete member initialization
            this.CurrentScene = currentScene;
        }

        public void LinkClicked(string linkId)
        {
            InteractiveScene interactiveScene = CurrentScene as InteractiveScene;
            if (null != interactiveScene)
            {
                IScene linkedScene = interactiveScene.LookupSceneByLinkId(linkId);
                if (null != linkedScene)
                {
                    OnNavigateRequest(linkedScene);
                }
                else
                {
                    OnPickSceneRequest(linkId);
                }
            }
        }

        private void OnPickSceneRequest(string linkId)
        {
            if (null != PickSceneRequest)
            {
                PickSceneRequest(this, linkId);
            }
        }

        private void OnNavigateRequest(IScene scene)
        {
            if (null != NavigateRequest)
            {
                NavigateRequest(this, scene);
            }
        }
    }
}
