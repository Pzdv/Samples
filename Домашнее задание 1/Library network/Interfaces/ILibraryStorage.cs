using Library_network.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_network.Interfaces
{
    public interface ILibraryStorage
    {
        public List<LibraryItem> Items { get; set; }
        void Add(LibraryItem item);
        void Remove(LibraryItem item);
    }
}
