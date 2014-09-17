using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.ViewModel
{
    public sealed class ScenePickerRequestArgs : EventArgs
    {
        private List<object> _senderChain = new List<object>();
        private string _linkId;

        public string LinkId
        {
            get { return _linkId; }
            set { _linkId = value; }
        }

        public List<object> SenderChain
        {
            get { return _senderChain; }
            set { _senderChain = value; }
        }

    }
}
