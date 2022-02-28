using Library_network.Models;
using Library_network.Storage;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests
{   
    [TestFixture]
    class LibraryStorageTests
    {
        [Test]
        public void LibraryStorageCtor_WithParam()
        {
            // Arrange
            var items = new List<LibraryItem>
            {
                new Book(1, "name"),
                new Patent(3, "name"),
                new Newspaper(2, "name")
            };

            var expectedItemCount = 3;

            // Act
            var storage = new LibraryStorage(items);

            // Assert
            Assert.AreEqual(expectedItemCount, storage.Items.Count);

        }
    }
}
