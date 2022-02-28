using Library_network.Models;
using Library_network.Reporters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_network.Reporters.ConcreteTextBuilders
{
    class NewspaperTextBuilder : IReportTextBuilder
    {
        public string CreateText(LibraryItem item)
        {
            var text = string.Empty;
            if (item is Newspaper newspaper)
            {
                text = $"{newspaper.Name} {newspaper.Date:dd.MM.yyyy}";
            }
            return text;
        }
    }
}
