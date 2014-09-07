using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryTeller.DataModel.Model;
using System.Collections.ObjectModel;

namespace StoryTeller
{
    public class LibraryViewModel
    {
        private ObservableCollection<LibraryItem> _libraryItems;
        public Library Library
        {
            get;
            set;
        }


        public ObservableCollection<LibraryItem> Items
        {
            get
            {
                if (_libraryItems == null)
                {
                    _libraryItems = new ObservableCollection<LibraryItem>(Library.Items);
                    _libraryItems.CollectionChanged += _libraryItems_CollectionChanged;
                }
                return _libraryItems;
            }
            }

        void _libraryItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Library.Items = Items.AsEnumerable();
        }
    }
}
