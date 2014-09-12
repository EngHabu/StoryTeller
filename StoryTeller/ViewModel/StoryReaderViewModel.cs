using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace StoryTeller.ViewModel
{
    public sealed class StoryReaderViewModel
    {
        public Story Story { get; set; }

        public IEnumerable<UIElement> Pages
        {
            get
            {
                yield break;
            }
        }

        public StoryReaderViewModel(Story story)
        {
            Story = story;
        }
    }

    internal sealed class SceneUIElementEnumerator : IEnumerator<UIElement>
    {
        public IScene Scene { get; set; }
        private List<UIElement> Elements { get; set; }

        private SceneUIElementEnumerator(IScene scene)
        {
            Scene = scene;
        }

        public UIElement Current
        {
            get { throw new NotImplementedException(); }
        }

        object System.Collections.IEnumerator.Current
        {
            get { throw new NotImplementedException(); }
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public static SceneUIElementEnumerator Create(IScene scene)
        {
            SceneUIElementEnumerator enumerator = new SceneUIElementEnumerator(scene);
            enumerator.GenerateElements();
            return enumerator;
        }

        private void GenerateElements()
        {
            throw new NotImplementedException();
        }


    }
}
