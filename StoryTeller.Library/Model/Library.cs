using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.DataModel.Model
{
    [DataContract]
    public sealed class Library
    {
        private static Library _current;

        [DataMember]
        public IEnumerable<LibraryItem> Items { get; set; }

        public static Library Current
        {
            get { return _current; }
            set { _current = value; }
        }

        public Library()
        {
            Items = new List<LibraryItem>();
        }
        
        internal static LibraryItem GetItem(string libraryItemId)
        {
            return (from item in Current.Items
                    where item.Id == libraryItemId
                    select item).FirstOrDefault();
        }      
    }
}
