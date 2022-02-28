using Library_network.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_network.Interfaces
{
    interface ILibrary
    {
        public void Add(LibraryItem item);
        public void Remove(LibraryItem item);
        public IEnumerable<LibraryItem> ShowAll();
        public IEnumerable<LibraryItem> FindByName(string name);
        public IEnumerable<LibraryItem> SortByPublishDate(bool ascending);
        public IEnumerable<Book> FindByAutor(string autor);
        public IEnumerable<IGrouping<string,Book>> FindBooksByPublisher(string publisher);
        public IEnumerable<IGrouping<int, LibraryItem>> GroupByPublishYear();
    }
}
