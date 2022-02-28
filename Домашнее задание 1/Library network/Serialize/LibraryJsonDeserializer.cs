using Library_network.Models;
using System;
using System.IO;
using System.Text.Json;
using Library_network.Serialize.Interfaces;
using System.Collections.Generic;
using Library_network.Interfaces;
using Library_network.Storage;

namespace Library_network.Serialize
{
    internal class LibraryJsonDeserializer : ILibraryDeserializer
    {
        public ILibraryStorage DeserializeIt(string path)
        {
            LibraryStorage storage = new(); 
            using (var sr = new StreamReader(path))
            {
                var jsonStrings = sr.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

                foreach (var jsonString in jsonStrings)
                {         
                    var itemType = GetItemType(jsonString);
                    var item = (LibraryItem)JsonSerializer.Deserialize(jsonString, itemType);
                    storage.Add(item);
                }  
            }
            return storage;
        }

        private static Type GetItemType(string jsonString)
        {
            var type = jsonString.Contains("ISBN") ? typeof(Book) :
                       jsonString.Contains("ISSN") ? typeof(Newspaper) :
                       jsonString.Contains("RegistrationNumber") ? typeof(Patent) :
                       throw new InvalidOperationException(message: "Данный тип не поддерживается для десереализации");
            return type;
        }
    }
}
