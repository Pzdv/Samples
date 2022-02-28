using Library_network.Interfaces;
using Library_network.Models;
using System.Collections.Generic;

namespace Library_network.Serialize.Interfaces
{
    internal interface ILibraryDeserializer
    {
        ILibraryStorage DeserializeIt(string path);
    }
}
