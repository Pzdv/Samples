using Library_network.Enums;

namespace Library_network.Interfaces
{
    public interface IDataManager
    {
        ILibraryStorage GetData(string path);
        void SaveData(ILibraryStorage storage, string path, SerializeTo type);
    }
}
