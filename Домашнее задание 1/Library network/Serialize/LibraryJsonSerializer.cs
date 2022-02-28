using Library_network.Serialize.Interfaces;
using Library_network.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Library_network.Interfaces;

namespace Library_network.Serializers
{
    internal class LibraryJsonSerializer : ILibrarySerializer
    {
        public void SerializeIt(ILibraryStorage storage, string filePath)
        {
            filePath = string.Concat(filePath, @"\data.json");

            foreach (var item in storage.Items)
            {
                var type = item.GetType().Name;

                switch (type)
                {
                    case "Book":
                        SerializeBook(filePath, item);
                        break;
                    case "Newspaper":
                        SerializeNewspaper(filePath, item);
                        break;
                    case "Patent":
                        SerializePatent(filePath, item);
                        break;
                    default:
                        throw new InvalidOperationException(message: "Для данного типа нет реализации сереализации");
                }
            }
        }

        private static void SerializeBook(string filePath, LibraryItem item)
        {
            using (var sw = new StreamWriter(filePath, true))
            {
                var jsonString = JsonSerializer.Serialize(item as Book);
                sw.WriteLine(jsonString);
            }
        }

        private static void SerializeNewspaper(string filePath, LibraryItem item)
        {
            using (var sw = new StreamWriter(filePath, true))
            {
                var jsonString = JsonSerializer.Serialize(item as Newspaper);
                sw.WriteLine(jsonString);
            }
        }

        private static void SerializePatent(string filePath, LibraryItem item)
        {
            using (var sw = new StreamWriter(filePath, true))
            {
                var jsonString = JsonSerializer.Serialize(item as Patent);
                sw.WriteLine(jsonString);
            }
        }
    }
}
