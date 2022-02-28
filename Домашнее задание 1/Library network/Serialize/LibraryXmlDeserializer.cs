using Library_network.Serialize.Interfaces;
using System.IO;
using System.Xml.Serialization;
using Library_network.Interfaces;
using Library_network.Storage;

namespace Library_network.Serializers
{
    internal class LibraryXmlDeserializer : ILibraryDeserializer
    {
        public ILibraryStorage DeserializeIt(string path)
        {
            LibraryStorage storage;

            using (var fs = new FileStream(path, FileMode.Open))
            {
                var xmlSerializer = new XmlSerializer(typeof(LibraryStorage));
                storage = (LibraryStorage)xmlSerializer.Deserialize(fs);

            }
            return storage;
        }
    }
}
