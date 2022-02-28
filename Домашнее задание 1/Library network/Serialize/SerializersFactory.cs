using Library_network.Enums;
using Library_network.Serialize;
using Library_network.Serialize.Interfaces;
using System;
using System.IO;

namespace Library_network.Serializers
{
    static internal class SerializersFactory
    {
        public static ILibraryDeserializer GetDeserializer(string fileExtension)
        {
            ILibraryDeserializer result = fileExtension switch
            {
                ".json" => new LibraryJsonDeserializer(),
                ".xml" => new LibraryXmlDeserializer(),
                _ => throw new InvalidDataException($"Формат {fileExtension} не поддерживается")
            };

            return result;
        }

        public static ILibrarySerializer GetSerializer(SerializeTo format)
        {
            return format switch
            {
                SerializeTo.Json => new LibraryJsonSerializer(),
                SerializeTo.Xml => new LibraryXmlSerializer(),
                _ => throw new InvalidOperationException(message: $"Для {format} нет реализации сереализации")
            };
        }
    }
}
