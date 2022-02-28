using FluentValidation.Results;
using Library_network.Models;
using Library_network.Validators;
using Library_network.Validators.ConcreteValidators;
using Library_network.Validators.Interfaces;
using NUnit.Framework;
using System.Linq;
using System.Text;

namespace Library.Tests
{
    internal class NewspaperValidatorTests
    {
        private NewspaperValidator _validator;
        private Newspaper _newspaper;
        private ValidationResult _newspaperValidationResult;

        [SetUp]
        public void SetUp()
        {
            _validator = new NewspaperValidator();

            var note = new StringBuilder();
            for (var i = 0; i < 501; i++)
            {
                note.Append('a');
            }

            _newspaper = new Newspaper(2, "")
            {
                PublishingHouse = string.Empty,
                PublishYear = 1899,
                PageCount = -1,
                Note = note.ToString(),
                Number = -1,
                InstanceCount = -1
            };
            _newspaperValidationResult = _validator.Validate(_newspaper);
        }

        [Test]
        public void ValidateNewspaper_NameIsEmpty_NameCannotBeEmpty()
        {
            Assert.That(_newspaperValidationResult.Errors.Any(x => x.PropertyName == "Name"));
        }

        [Test]
        public void ValidateNewspaper_NameIsNull_NameCannotBeNull()
        {
            _newspaper.Name = null;

            var newspaperValidationResult = _validator.Validate(_newspaper);

            Assert.That(newspaperValidationResult.Errors.Any(x => x.PropertyName == "Name"));
        }

        [Test]
        public void ValidateNewspaper_PublishingHouseIsEmpty_PublishingHouseCannotBeEmpty()
        {
            Assert.That(_newspaperValidationResult.Errors.Any(x => x.PropertyName == "PublishingHouse"));
        }

        [Test]
        public void ValidateNewspaper_PublishingHouseIsNull_PublishingHouseCannotBeNull()
        {
            _newspaper.PublishingHouse = null;

            var newspaperValidationResult = _validator.Validate(_newspaper);

            Assert.That(newspaperValidationResult.Errors.Any(x => x.PropertyName == "PublishingHouse"));
        }

        [Test]
        public void ValidateNewspaper_PublishYearIs1899_PublishYearCannotBeLessThan1900()
        {
            Assert.That(_newspaperValidationResult.Errors.Any(x => x.PropertyName == "PublishYear"));
        }

        [Test]
        public void ValidateNespaper_PageCountLessThan1_PageCountCannotBeLessThan1()
        {
            Assert.That(_newspaperValidationResult.Errors.Any(x => x.PropertyName == "PageCount"));
        }

        [Test]
        public void ValidateNewspaper_NotLengthis501_NotLengthCannotBeGreaterThan500()
        {
            Assert.That(_newspaperValidationResult.Errors.Any(x => x.PropertyName == "Note"));
        }

        [Test]
        public void ValidateNewspaper_NumberIsNegative_NumberCannotBeNegatice()
        {
            Assert.That(_newspaperValidationResult.Errors.Any(x => x.PropertyName == "Number"));
        }

        [Test]
        public void ValidateNewspaper_DateIsEmpty_DateCannotBeEmpty()
        {
            Assert.That(_newspaperValidationResult.Errors.Any(x => x.PropertyName == "Date"));
        }

        [Test]
        public void ValidateNewspaper_DateIsNull_DateCannotBeNull()
        {
            _newspaper.Date = null;

            var newspaperValidationResult = _validator.Validate(_newspaper);

            Assert.That(newspaperValidationResult.Errors.Any(x => x.PropertyName == "Date"));
        }

        [Test]
        public void ValidateNespaper_PriceIsEmpty_PriceCannotBeEmpty()
        {
            Assert.That(_newspaperValidationResult.Errors.Any(x => x.PropertyName == "Price"));
        }

        [Test]
        public void ValidateNewspaper_PriceIsNegatice_PriceCannotBeNegative()
        {
            _newspaper.Price = -1;

            var validationResult = _validator.Validate(_newspaper);

            Assert.That(validationResult.Errors.Any(x => x.PropertyName == "Price"));
        }

        [Test]
        public void ValidateNewspaper_InstanceCountIsNegative_InstanceCountCannotBeNegatice()
        {
            Assert.That(_newspaperValidationResult.Errors.Any(x => x.PropertyName == "InstanceCount"));
        }

        [TestCase("2049-3630", false)]
        [TestCase("20493630", true)]
        [TestCase("204-93630", true)]
        public void ValidateNewspaper_IssnFormat(string issn, bool expected)
        {
            _newspaper.ISSN = issn;

            var validationResult = _validator.Validate(_newspaper);
            var actual = validationResult.Errors.Any(x => x.PropertyName == "ISSN");

            Assert.AreEqual(expected, actual);     
        }

    }
}
