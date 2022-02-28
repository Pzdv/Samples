using Library_network.Interfaces;
using Library_network.Models;
using Library_network.Reporters.ConcreteTextBuilders;
using Library_network.Reporters.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_network.Reporters
{
    public class TextReporter : IReporter
    {
        private IReportTextBuilder _textBuilder;

        public void CreateReport(IEnumerable<LibraryItem> libraryItems,string path, string name)
        {
            var filePath = string.Concat(path, "\\", name, ".txt");

            var grouppedItems = libraryItems.GroupBy(x => x.GetType());

            var stringBuilder = new StringBuilder();

            using var streamWriter = new StreamWriter(filePath, false);
            foreach (var type in grouppedItems)
            {
                _textBuilder = TextBuilderCreater.CreateTextBuilder(type.Key);

                var header = _textBuilder.CreateHeader(type.Key.Name);
                stringBuilder.Append(header);

                foreach (var item in type)
                {
                    var body = _textBuilder.CreateText(item);
                    stringBuilder.Append(body);
                }

                var footer = _textBuilder.CreateFooter(type.Count());
                stringBuilder.Append(footer);
                streamWriter.Write(stringBuilder.ToString());
                stringBuilder.Clear();
            }
        }
    }
}
