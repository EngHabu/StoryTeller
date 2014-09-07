using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.DataModel.Model
{
    public sealed class Library
    {
        public Library ()
        {
            Items = new List<LibraryItem>();
        }
        public IEnumerable<LibraryItem> Items { get; set; }

        internal static LibraryItem GetItem(string libraryItemId)
        {
            throw new NotImplementedException();
        }      
    }
}
