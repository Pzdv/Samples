using Library_network.Models;
using Library_network.Validators;
using Lib = Library_network.Library;
using Library_network.Storage;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Library_network.Reporters;
using System.IO;
using System.Text;
using Library_network.CustomEventArgs;
using System.Text.Json;
using Library_network.Serializers;

namespace Library.Tests
{
    [TestFixture]
    public class LibraryTests
    {
        private Lib.Library _library;
        private LibraryStorage _storage;
        private readonly Book eventTestBook = new(10, "EventTest");
        private readonly Newspaper eventTestNewspaper = new(11, "EventTest");
        private readonly Patent eventTestPatent = new(12, "EventTest");

        [SetUp]
        public void Setup()
        {
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

            _storage = new LibraryStorage();
            _storage.Add(Book4);
            _storage.Add(Book3);
            _storage.Add(Book2);
            _storage.Add(Book1);
            _storage.Add(patent2);
            _storage.Add(patent1);

            _library = new Lib.Library(_storage);
        }

        [Test]
        public void Add_AddNewLibraryItem_RightCountLibraryItemsReturned()
        {
            // Arrange
            var newspaper = new Newspaper(7, "Name");
            var excpectedLibrartItems = _library.ShowAll().Count() + 1;

            // Act
            _library.Add(newspaper);
            var actualResult = _library.ShowAll().Count();

            // Assert
            Assert.AreEqual(excpectedLibrartItems, actualResult);
        }

        [Test]
        public void Add_AddItemWithExistID_InvalidOperationExceptionReturned()
        {
            // Arrange
            var book = new Book(1, "name");
            // Act

            // Assert
            Assert.Throws<InvalidOperationException>(() => _library.Add(book));
        }

        [TestCase(0, 1)]
        [TestCase(1, 2)]
        [TestCase(4, 5)]
        public void Remove_RemoveLibraryItem_LibraryItemWasRemoved(int removedIndex, int expectedIndex)
        {
            // Arrange
            var removingItem = _library.ShowAll().ToList()[removedIndex];
            var expectedItem = _library.ShowAll().ToList()[expectedIndex];

            // Act
            _library.Remove(removingItem);
            var actualResult = _library.ShowAll().ToList()[removedIndex];

            //Assert
            Assert.AreEqual(expectedItem, actualResult);
        }

        [Test]
        public void ShowAll_UseShowAll_AllItemsReturned()
        {
            // Arrange
            var expectedItemsCount = _storage.Items.Count;

            // Act
            var actualItemsCount = _library.ShowAll().Count();

            // Assert
            Assert.AreEqual(expectedItemsCount, actualItemsCount);
        }

        [Test]
        public void FindByName_TryFindByName_RightAmountOfItemsReturned()
        {
            // Arrange
            var expected = 3;

            // Act
            var actual = _library.FindByName("Other").Count();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FindByName_TryFindExistItem_RightItemReturned()
        {
            // Arrange
            var requiredName = "Other";

            // Act
            var actual = _library.FindByName(requiredName);

            //Assert
            foreach (var item in actual)
            {
                Assert.That(item.Name.Contains(requiredName));
            }
        }

        [Test]
        public void FindByName_TryFindNotExistItem_EmptyArrayReturned()
        {
            // Arrange
            var requiredName = "Another";

            // Act
            var actualResult = _library.FindByName(requiredName).Count();

            // Assert
            Assert.That(actualResult == 0);
        }

        [Test]
        public void GroupByYear_3DifferentYears_3GroupReturned()
        {
            // Arrange
            var expectedGroups = 3;

            // Act
            var actualGroups = _library.GroupByPublishYear().Count();

            //Assert
            Assert.AreEqual(expectedGroups, actualGroups);
        }

        [Test]
        public void GroupByYear_3DifferentYears_GroupsHaveSameYears()
        {
            // Act
            var groupedItems = _library.GroupByPublishYear();

            // Assert
            foreach (var group in groupedItems)
            {
                foreach (var element in group)
                {
                    Assert.That(group.Key == element.PublishYear);
                }
            }
        }

        [Test]
        public void FindByAutor_AutorNotInList_EmptyArrayReturned()
        {
            // Arrange

            // Act
            var result = _library.FindByAutor("AnotherAutor");
            var actualResult = result.Count();
            // Assert
            Assert.That(actualResult == 0);
        }

        [Test]
        public void FindByAutor_3ItemWithCertainAutor_3ItemReturned()
        {
            // Arrange
            var expectedAutor = "autor";
            // Act
            var actualResult = _library.FindByAutor(expectedAutor).Count(); 
            // Assert
            Assert.AreEqual(3, actualResult);
        }

        [Test]
        public void FindByAutor_3ItemWithCertainAutor_3RightItemReturned()
        {
            //Arrange
            var expectedAutor = "autor";
            //Act
            var actualResult = _library.FindByAutor(expectedAutor);
            //Assert
            foreach (var book in actualResult)
            {
                var isContains = false;

                if (book.Autors.Contains(expectedAutor))
                    isContains = true;

                Assert.That(isContains);
            }
        }

        [Test]
        public void FindBooksByPublisher_TryFindPublisher_3RightGroupReturned()
        {
            // Arrange
            var requiredPublisher = "publisher";
            // Act
            var actualResult = _library.FindBooksByPublisher(requiredPublisher);
            // Assert

            foreach (var books in actualResult)
            {
                foreach (var book in books)
                {
                    Assert.That(book.PublishingHouse.StartsWith(requiredPublisher));
                }
            }
        }

        [Test]
        public void FindBooksByPublisher_TryFindPublisher_3GroupReturned()
        {
            // Arrange
            var requiredPublisher = "publisher";
            var expectedGroups = 3;
            // Act
            var actualResult = _library.FindBooksByPublisher(requiredPublisher).Count();
            // Assert
            Assert.AreEqual(expectedGroups, actualResult);
        }

        [Test]
        public void FindBooksByPublisher_TryFindNotExistPublisher_EmptyArrayReturned()
        {
            // Arrange
            var requiredPublisher = "newPublisher";
            // Act
            var actualResult = _library.FindBooksByPublisher(requiredPublisher).Count();
            // Assert
            Assert.That(actualResult == 0);
        }

        [TestCase(true, 1999)]
        [TestCase(false, 2001)]
        public void SortByPublishYear_AscendingAndDescending(bool ascending, int expected)
        {
            // Arrange

            // Act
            var actual = _library.SortByPublishDate(ascending).First().PublishYear;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Sort_SortByIdTypeBook_FirstItemWithMinIdReturned()
        {
            // Arrange
            var expected = _library.ShowAll().OrderBy(x => x.Id);

            // Act
            var actual = _library.Sort<LibraryItem, int>(x => x.Id);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FilterBy_FilterByName_ReturnAllElementWithExpectedName()
        {
            // Arrange
            var excpected = _library.ShowAll().Where(x => x.Name == "Name");

            // Act
            var actual = _library.FilterBy<LibraryItem>(x => x.Name == "Name");

            // Assert
            Assert.AreEqual(excpected, actual);
        }

        [Test]
        public void EditLibraryItem_ChangeNameToNewName_PropertyWasChanged()
        {
            // Arrange
            var book = new Book(10, "oldName");
            var newName = "newName";

            // Act
            _library.EditLibraryItem(book, "Name", newName);

            // Assert
            Assert.AreEqual(newName, book.Name);
        }

        [Test]
        public void EditLibraryItem_ChangeSeveralProperties()
        {
            // Arrange
            var newAutors = new List<string> { "newAutor1", "newAutor2" };
            var newInstanceCount = 10;
            var newISBN = "978-3-16-148410-1";
            var newPrice = 400m;
            var newPropertiesValue = new Dictionary<string, object>()
            {
                { "Autors",  newAutors},
                { "InstanceCount", newInstanceCount },
                { "ISBN", newISBN },
                { "Price", newPrice }
            };
            var book = new Book(1, "Book") { Autors = new List<string> { "autor1", "autor2" }, InstanceCount = 100, ISBN = "978-3-16-148410-0", Price = 500 };
            // Act
            _library.EditLibraryItem(book, newPropertiesValue);

            // Assert
            Assert.AreEqual(newAutors, book.Autors);
            Assert.AreEqual(newInstanceCount, book.InstanceCount);
            Assert.AreEqual(newISBN, book.ISBN);
            Assert.AreEqual(newPrice, book.Price);
        }

        [Test]
        public void CreateReport_ReportContainsAllItem()
        {
            // Arrange
            var directory = Environment.CurrentDirectory;
            var filename = "Report";
            var reporter = new TextReporter();

            // Act
            _library.CreateReport(filename, directory, reporter);
            var report = string.Empty;
            var filePath = string.Concat(directory, "\\", filename, ".txt");
            using (var streamReader = new StreamReader(filePath))
            {
                report = streamReader.ReadToEnd();
            }

            // Assert
            foreach (var item in _library.ShowAll())
            {
                Assert.That(report.Contains(item.Name));
            }

            var fileinfo = new FileInfo(filePath);
            if (fileinfo.Exists)
            {
                fileinfo.Delete();
            }
        }

        [Test]
        public void FundAdded_AddNewBook_HaveAddedItemType()
        {
            // Arrange
            Type actual = null;
            var expected = typeof(Book);
            _library.FundAdded += delegate (object sender, AddedItemEventArgs e)
            {
                actual = e.ItemType;
            };

            // Act
            _library.Add(eventTestBook);

            // Assert
            Assert.AreEqual(expected, actual);    
        }

        [Test]
        public void FundAdded_AddNewNewspaper_HaveAddedItemType()
        {
            // Arrange
            Type actual = null;
            var expected = typeof(Newspaper);
            _library.FundAdded += delegate (object sender, AddedItemEventArgs e)
            {
                actual = e.ItemType;
            };

            // Act
            _library.Add(eventTestNewspaper);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FundAdded_AddNewPatent_HaveAddedItemType()
        {
            // Arrange
            Type actual = null;
            var expected = typeof(Patent);
            _library.FundAdded += delegate (object sender, AddedItemEventArgs e)
            {
                actual = e.ItemType;
            };

            // Act
            _library.Add(eventTestPatent);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FundDeleting_DeleteBook_HaveDeletingItemType()
        {
            // Arrange
            LibraryItem actual = null;
            var expected = eventTestBook;
            _library.FundDeleting += delegate (object sender, DeletingItemEventArgs e)
            {
                actual = e.DeletingItem;
            };

            // Act
            _library.Remove(eventTestBook);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FundDeleting_DeleteNewspaper_HaveDeletingItemType()
        {
            // Arrange
            LibraryItem actual = null;
            var expected = eventTestNewspaper;
            _library.FundDeleting += delegate (object sender, DeletingItemEventArgs e)
            {
                actual = e.DeletingItem;
            };

            // Act
            _library.Remove(eventTestNewspaper);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FundDeleting_DeletePatent_HaveDeletingItemType()
        {
            // Arrange
            LibraryItem actual = null;
            var expected = eventTestPatent;
            _library.FundDeleting += delegate (object sender, DeletingItemEventArgs e)
            {
                actual = e.DeletingItem;
            };

            // Act
            _library.Remove(eventTestPatent);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FundDeleting_CancelDeletion_DeletionWasAborted()
        {
            // Arrange
            _library.Add(eventTestBook);
            _library.FundDeleting += delegate (object sender, DeletingItemEventArgs e)
            {
                if (e.DeletingItem == eventTestBook)
                {
                    e.Abort = true;
                }
            };

            // Act
            _library.Remove(eventTestBook);
            var isContains = _library.ShowAll().Contains(eventTestBook);
            // Assert
            Assert.That(isContains);
        }

        [TestCase(true)]
        public void Load_LoadInvalidItems_AllItemsWasLoaded(bool loadInvalidItems)
        {
            // Arrange 
            _storage.Items = null;
            var validator = new LibraryFluentValidator();
            var path = Environment.CurrentDirectory + @"\TestData\TestingData.json";
            var expectedItems = new DataManager().GetData(path).Items.OrderBy(x => x.Id).ToList();

            // Act
            _library.Load(validator, path, loadInvalidItems);
            var actual = _library.ShowAll().OrderBy(x => x.Id).ToList();
            // Assert
            CollectionAssert.AreEqual(expectedItems, actual);
        }

        [TestCase(false)]
        public void Load_LoadInvalidItems_InvalidOperationException(bool loadInvalidItems)
        {
            // Arrange 
            _storage.Items = null;
            var validator = new LibraryFluentValidator();
            var path = Environment.CurrentDirectory + @"\TestData\TestingData.json";

            // Assert
            Assert.Throws<InvalidOperationException>(() => _library.Load(validator, path, loadInvalidItems));
        }

        [Test]
        public void Load_LoadInvalidItems_AllErrorsAreRecordedInLogFile()
        {
            // Arrange 
            _storage.Items = null;
            var validator = new LibraryFluentValidator();
            var dataPath = Environment.CurrentDirectory + @"\TestData\TestingData.json";
            var expectedRecordsCount = new DataManager().GetData(dataPath).Items.OrderBy(x => x.Id).ToList().Count;
            var recordsCount = 0;
            // Act
            _library.Load(validator, dataPath, true);
            var logFolder = Environment.CurrentDirectory + @"\testlogs";
            var logPath = Directory.GetFiles(logFolder)[0];
            using (var sr = new StreamReader(logPath))
            {
                recordsCount = sr.ReadToEnd().Split("id:").Length - 1;
            }

            // Assert
            Assert.AreEqual(expectedRecordsCount, recordsCount);
        }

        [TearDown]
        public void TearDown()
        {
            var path = Environment.CurrentDirectory + @"\testlogs";
            if(Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            path = Environment.CurrentDirectory + @"\Tests";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
    }
}