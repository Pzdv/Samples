using Library_network.Reporters.Interfaces;
using Library_network.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_network.Reporters.ConcreteTextBuilders
{
    class BookTextBuilder : IReportTextBuilder
    {
        public string CreateText(LibraryItem item)
        {
            var text = string.Empty;
            if (item is Book book)
            {
                text = $"{string.Join(", ", book.Autors)}    {book.Name} \n";
            }
            
            return text;
        }
    }
}
