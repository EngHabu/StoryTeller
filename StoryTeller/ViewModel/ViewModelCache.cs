using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.ViewModel
{
    public sealed class ViewModelCache
    {
        private static ViewModelCache _local;
        private static readonly object _syncObject = new object();

        private Dictionary<string, object> _viewModels = new Dictionary<string, object>();

        public static ViewModelCache Local
        {
            get
            {
                if (null == _local)
                {
                    lock (_syncObject)
                    {
                        if (null == _local)
                        {
                            _local = new ViewModelCache();
                        }
                    }
                }

                return _local;
            }
        }

        public void Put(string key, object value)
        {
            _viewModels[key] = value;
        }

        public object Retrieve(string key)
        {
            object result;
            if (!_viewModels.TryGetValue(key, out result))
            {
                result = null;
            }

            return result;
        }
    }
}
