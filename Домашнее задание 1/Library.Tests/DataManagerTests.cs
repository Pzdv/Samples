using Library_network.Models;
using Lib = Library_network.Library;
using Library_network.Storage;
using NUnit.Framework;
using System;
using Library_network.Serializers;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Library_network.Enums;
using Library_network.Validators;
using System.Xml.Serialization;
using System.Xml;

namespace Library.Tests
{
    [TestFixture]
    class DataManagerTests
    {
        private readonly DataManager _dataManager = new();

        private Lib.Library _library;
        private LibraryStorage _storage;
        private string _path;
        [SetUp]
        public void Setup()
        {
            _path = string.Concat(Environment.CurrentDirectory, "\\Tests");
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration("NLog.config");
            var Book1 = new Book(1, "Name")
            {
                Autors = new List<string> { "autor" },
                PublishingHouse = "publisher",
                PublishYear = 1999,
                CreationCity = "Izhevsk"
            };

            var Book2 = new Book(2, "Name2")
            {
                Autors = new List<string> { "autor", "autor2" },
                PublishingHouse = "publisher2",
                PublishYear = 1999
            };

            var Book3 = new Book(3, "Other")
            {
                Autors = new List<string> { "autor2", "autor3" },
                PublishingHouse = "publisher3",
                PublishYear = 2000
            };

            var Book4 = new Book(4, "OtherName")
            {
                Autors = new List<string> { "autor", "autor3" },
                PublishingHouse = "4publisher",
                PublishYear = 2001
            };

            var patent1 = new Patent(5, "N")
            {
                PublishYear = 1999
            };

            var patent2 = new Patent(6, "OtherN2")
            {
                PublishYear = 2000
            };

            var newspaper = new Newspaper(7, "Newspaper")
            {
                PublishYear = 2000
            };

            _storage = new LibraryStorage();
            _storage.Add(Book4);
            _storage.Add(Book3);
            _storage.Add(Book2);
            _storage.Add(Book1);
            _storage.Add(patent2);
            _storage.Add(patent1);
            _storage.Add(newspaper);

            _library = new Lib.Library(_storage);
        }

        [TestCase(SerializeTo.Json)]
        public void SaveData_JsonSerializeAllItems_ShouldSerializedAllItems(SerializeTo format)
        {
            // Arrange
            var itemsCount = _storage.Items.Count();

            // Act
            _dataManager.SaveData(_storage, _path, format);
            var fileWithSerializedItemsPath = Directory.GetFiles(_path)[0];
            var rowsCount = 0;
            using (var sr = new StreamReader(fileWithSerializedItemsPath))
            {
                rowsCount = sr.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Length;
            }

            // Assert
            Assert.AreEqual(itemsCount, rowsCount);
        }

        [TestCase(SerializeTo.Xml)]
        public void SaveData_XmlSerializeAllItems_ShouldSerializedAllItems(SerializeTo format)
        {
            // Arrange
            var itemsCount = _storage.Items.Count;

            // Act
            _dataManager.SaveData(_storage, _path, format);
            var fileWithSerializedItemsPath = Directory.GetFiles(_path)[0];
            var serializedItemsCount = 0;
            using (var fs = new FileStream(fileWithSerializedItemsPath, FileMode.Open))
            {
                var xmlSerializer = new XmlSerializer(typeof(LibraryStorage));
                var deserializedItems = (LibraryStorage)xmlSerializer.Deserialize(fs);
                serializedItemsCount = deserializedItems.Items.Count;
            }

            // Assert
            Assert.AreEqual(itemsCount, serializedItemsCount);
        }

        [TestCase(SerializeTo.Xml)]
        public void GetData_XmlDeserializeItems_OriginalItemsAndDeserializedItemsShouldBeEqual(SerializeTo format)
        {
            // Arrange 
            var expected = _storage.Items.OrderBy(x => x.Id).ToList();

            _dataManager.SaveData(_storage, _path, format);
            _path += @"\data.xml";
            // Act
            var actual = _dataManager.GetData(_path).Items.OrderBy(x => x.Id).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }
        [TestCase(SerializeTo.Json)]
        public void GetData_JsonDeserializeItems_OriginalItemsAndDeserializedItemsShouldBeEqual(SerializeTo format)
        {
            // Arrange 
            var expected = _library.ShowAll().OrderBy(x => x.Id).ToList();

            _dataManager.SaveData(_storage, _path, format);
            _path += @"\data.json";

            // Act
            var actual = _dataManager.GetData(_path).Items.OrderBy(x => x.Id).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
