using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.DataModel.Model
{
    public sealed class Library
    {
        public IEnumerable<LibraryItem> Items { get; set; }

        internal static LibraryItem GetItem(string _libraryItemId)
        {
            throw new NotImplementedException();
        }
    }
}
