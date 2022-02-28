using Library_network.Interfaces;
using Library_network.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Library_network.Storage
{
    public class LibraryStorage : ILibraryStorage

    {
        private List<LibraryItem> _items;

        [XmlArray("Items")]
        [XmlArrayItem("Book", typeof(Book))]
        [XmlArrayItem("Patent", typeof(Patent))]
        [XmlArrayItem("Newspaper", typeof(Newspaper))]
        public List<LibraryItem> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
            }
        }

        public LibraryStorage() 
        {
            _items = new();
        }

        public LibraryStorage(IEnumerable<LibraryItem> items)
        {
            _items = new();
            foreach (var item in items)
            {
                _items.Add(item);
            }
        }

        public void Add(LibraryItem item)
        {
            _items.Add(item);
        }

        public void Remove(LibraryItem item)
        {
            _items.Remove(item);
        }
    }
}
