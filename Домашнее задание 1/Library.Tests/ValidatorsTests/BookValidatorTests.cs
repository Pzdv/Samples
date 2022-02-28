using FluentValidation.Results;
using Library_network.Models;
using Library_network.Validators;
using Library_network.Validators.ConcreteValidators;
using Library_network.Validators.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests
{
    internal class BookValidatorTests
    {
        private BookValidator _validator;
        private Book _book;
        private ValidationResult _bookValidationResult;

        [SetUp]
        public void SetUp()
        {
            _validator = new BookValidator();
            var note = new StringBuilder();
            for (var i = 0; i < 501; i++)
            {
                note.Append('a');
            }

            _book = new Book(1, "")
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

            _bookValidationResult = _validator.Validate(_book);
        }

        [Test]
        public void ValidateBook_NameIsEmpty_NameCannotBeEmpty()
        {
            Assert.That(_bookValidationResult.Errors.Any(x => x.PropertyName == "Name"));
        }

        [Test]
        public void ValidateBook_AutorsIsNull_AutorsCannotBeEmpty()
        {
            Assert.That(_bookValidationResult.Errors.Any(x => x.PropertyName == "Autors"));
        }

        [Test]
        public void ValidateBook_AutorsIsEmpty_AutorsCannotBeEmpty()
        {
            _book.Autors = new List<string>();

            var validationResult = _validator.Validate(_book);

            Assert.That(validationResult.Errors.Any(x => x.PropertyName == "Autors"));
        }

        [Test]
        public void ValidateBook_CreationCityIsEmpty_CreationCityCannotBeEmpty()
        {
            Assert.That(_bookValidationResult.Errors.Any(x => x.PropertyName == "CreationCity"));
        }

        [Test]
        public void ValidateBook_PublishingHouseIsEmpty_PublishingHouseCannotBeEmpty()
        {
            Assert.That(_bookValidationResult.Errors.Any(x => x.PropertyName == "PublishingHouse"));
        }

        [Test]
        public void ValidateBook_PublishYearIs1899_PublishYearShouldNotBeLessThan1900()
        {
            Assert.That(_bookValidationResult.Errors.Any(x => x.PropertyName == "PublishYear"));
        }

        [Test]
        public void ValidateBook_PageCountLessThan1_PageCountShouldNotBeLessThan1()
        {
            Assert.That(_bookValidationResult.Errors.Any(x => x.PropertyName == "PageCount"));
        }

        [Test]
        public void ValidateBook_NoteLengthGreaterThan500_NoteLengthShouldNotBeGreaterThan500()
        {
            Assert.That(_bookValidationResult.Errors.Any(x => x.PropertyName == "Note"));
        }

        [Test]
        public void ValidateBook_PriceIsNegative_PriceCannotBeNegative()
        {
            Assert.That(_bookValidationResult.Errors.Any(x => x.PropertyName == "Price"));
        }

        [Test]
        public void ValidateBook_PriceIsEmpty_PriceCannotBeEmpty()
        {
            var book = new Book(1, "");

            var validationResult = _validator.Validate(book);

            Assert.That(validationResult.Errors.Any(x => x.PropertyName == "Price"));
        }

        [Test]
        public void ValidateBook_InstanceCountIsNegative_InstanceCountCannotBeNegative()
        {
            Assert.That(_bookValidationResult.Errors.Any(x => x.PropertyName == "InstanceCount"));
        }

        [TestCase("978-3-13-138410-2", false)]
        [TestCase("978-3-13-1384102", true)]
        [TestCase("9783131384102", true)]
        public void ValidateBook_IsbnFormat_IsbnFormatMustBeRight(string isbn, bool expected)
        {
            _book.ISBN = isbn;

            var validationResult = _validator.Validate(_book);
            var actual = validationResult.Errors.Any(x => x.PropertyName == "ISBN");

            Assert.AreEqual(expected, actual);
        }
    }
}
