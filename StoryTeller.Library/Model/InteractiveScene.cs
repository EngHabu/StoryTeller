using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.DataModel.Model
{
    [DataContract]
    public sealed class InteractiveScene : Scene
    {
        private IList<IScene> _possibleScenes = new List<IScene>();
        private Dictionary<string, string> _linkIdToSceneId = new Dictionary<string, string>();

        public InteractiveScene(LibraryItem libraryItem)
            : base(libraryItem)
        {
        }

        [DataMember]
        public override IScene FollowingScene
        {
            get
            {
                if (PossibleScenes.Count == 1)
                {
                    return PossibleScenes[0];
                }

                return null;
            }
            set
            {
                // Do nothing
            }
        }

        [DataMember]
        public Dictionary<string, string> LinkIdToSceneId
        {
            get { return _linkIdToSceneId; }
            set { _linkIdToSceneId = value; }
        }

        [DataMember]
        public IList<IScene> PossibleScenes
        {
            get { return _possibleScenes; }
            set { _possibleScenes = value; }
        }

        public IScene LookupSceneByLinkId(string linkId)
        {
            IScene result = null;
            string sceneId;
            if (LinkIdToSceneId.TryGetValue(linkId, out sceneId))
            {
                result = PossibleScenes.Where((scene) => scene.Id == sceneId).FirstOrDefault();
            }

            return result;
        }
    }
}
