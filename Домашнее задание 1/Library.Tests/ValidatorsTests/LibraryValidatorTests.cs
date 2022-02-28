using Lib = Library_network.Library;
using Library_network.Models;
using Library_network.Validators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library_network.Storage;
using FluentValidation.Results;
using Library_network.Validators.Interfaces;

namespace Library.Tests
{
    [TestFixture]
    class LibraryValidatorTests
    {
        private ILibraryValidator _validator;
        private Lib.Library _library;

        [SetUp]
        public void SetUp()
        {
            _validator = new LibraryFluentValidator();
            _library = new(new LibraryStorage());

            var note = new StringBuilder();
            for (var i = 0; i < 501; i++)
            {
                note.Append('a');
            }
            var book = new Book(1, "")
            {
                Autors = null,
                CreationCity = string.Empty,
                PublishingHouse = string.Empty,
                PublishYear = 1899,
                PageCount = 0,
                Note = note.ToString(),
                Price = -1,
                InstanceCount = -1
            };
            var newspaper = new Newspaper(2, "")
            {
                PublishingHouse = string.Empty,
                PublishYear = 1899,
                PageCount = -1,
                Note = note.ToString(),
                Number = -1,
                Date = default,
                InstanceCount = -1
            };
            var patent = new Patent(3, "")
            {
                Inventors = new List<string>(),
                Country = string.Empty,
                PageCount = 0,
                Note = note.ToString(),
                Price = -1
            };
            
            _library.Add(patent);
            _library.Add(newspaper);
            _library.Add(book);
        }
        [Test]
        public void ValidateAllItems_ItemsIsInvalid_AllInvalidItemsDetected()
        {
            var results = _library.Validate(_validator);
            Assert.That(results.Count() == 3);
        }
    }
}
