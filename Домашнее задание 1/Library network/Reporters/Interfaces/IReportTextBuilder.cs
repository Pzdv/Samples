using Library_network.Models;

namespace Library_network.Reporters.Interfaces
{
    interface IReportTextBuilder
    {
        public string CreateFooter(int itemsCount)
        {
            return $"{itemsCount}\n\n";
        }

        public string CreateHeader(string type)
        {
            return $"{type}\n";
        }

        string CreateText(LibraryItem item);
    }
}
