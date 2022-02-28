using Library_network.Models;
using System.Collections.Generic;
using Library_network.Interfaces;

namespace Library_network.Serialize.Interfaces
{
    internal interface ILibrarySerializer
    {
        void SerializeIt(ILibraryStorage storage, string path);
    }
}
