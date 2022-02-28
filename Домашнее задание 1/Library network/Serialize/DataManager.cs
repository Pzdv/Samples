using Library_network.Enums;
using Library_network.Interfaces;
using Library_network.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace Library_network.Serializers
{
    public class DataManager : IDataManager 
    {
        public ILibraryStorage GetData(string path)
        {
            var extension = Path.GetExtension(path);
            if(extension == ".xml")
            {
                XsdValidate(path, "XMLSchemaUploadingData.xsd");
            }

            var deserializer = SerializersFactory.GetDeserializer(extension);

            var result = deserializer.DeserializeIt(path);
            return result;
        }

        public void SaveData(ILibraryStorage storage, string path, SerializeTo format)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);

            var serializer = SerializersFactory.GetSerializer(format);

            serializer.SerializeIt(storage, path);
        }

        private void XsdValidate(string xmlPath, string xsdPath)
        {
            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema
            };

            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

            settings.Schemas.Add("", xsdPath);

            settings.ValidationEventHandler += Settings_ValidationEventHandler;

            using (var reader = XmlReader.Create(xmlPath, settings))
            {
                while (reader.Read())
                {
                }
            }
        }

        private void Settings_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            throw new Exception(message: $"Xml документ не соответсвует xsd схеме.{Environment.NewLine}{e.Message}");
        }
    }
}
