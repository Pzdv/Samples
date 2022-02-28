using Library_network.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_network.Reporters.Interfaces
{
    public interface IReporter
    {
        void CreateReport(IEnumerable<LibraryItem> libraryItems, string path, string name);
    }
}
