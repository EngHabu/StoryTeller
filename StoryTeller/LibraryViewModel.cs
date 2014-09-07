using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryTeller.DataModel.Model;

namespace StoryTeller
{
    public class LibraryViewModel
    {

        private Library _library;

        public LibraryViewModel(Library library)
        {
            _library = library;
        }

        public IEnumerable<LibraryItem> Items
        {
            get
            {
                return _library.Items;
            }
        }
    }
}
