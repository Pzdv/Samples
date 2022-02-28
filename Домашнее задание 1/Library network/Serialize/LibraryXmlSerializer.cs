using Library_network.Serialize.Interfaces;
using Library_network.Models;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using Library_network.Interfaces;

namespace Library_network.Serializers
{
    internal class LibraryXmlSerializer : ILibrarySerializer
    {
        public void SerializeIt(ILibraryStorage storage, string filePath)
        {
            filePath = string.Concat(filePath, @"\data.xml");
            using (var sw = new StreamWriter(filePath, true))
            {  
                var xmlSerializer = new XmlSerializer(storage.GetType());
                xmlSerializer.Serialize(sw, storage);
            }
        }
    }
}
