using FluentValidation.Results;
using Library_network.Models;
using Library_network.Validators.ConcreteValidators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests
{
    internal class PatentValidatorTests
    {
        private PatentValidator _validator;
        private Patent _patent;
        private ValidationResult _patentValidationResult;

        [SetUp]
        public void SetUp()
        {
            _validator = new PatentValidator();

            var note = new StringBuilder();
            for (var i = 0; i < 501; i++)
            {
                note.Append('a');
            }

            _patent = new Patent(3, "")
            {
                Inventors = new List<string>(),
                Country = string.Empty,
                PageCount = 0,
                Note = note.ToString()
            };

            _patentValidationResult = _validator.Validate(_patent);
        }
        [Test]
        public void ValidatePatent_NameIsEmpty_NameCannotBeEmpty()
        {
            Assert.That(_patentValidationResult.Errors.Any(x => x.PropertyName == "Name"));
        }

        [Test]
        public void ValidatePatent_NameIsNull_NameCannotBeNull()
        {
            _patent.Name = null;

            var validationResult = _validator.Validate(_patent);

            Assert.That(validationResult.Errors.Any(x=>x.PropertyName=="Name"));
        }

        [Test]
        public void ValidatePatent_InventorsIsEmpty_InventorsCannotBeEmpty()
        {
            Assert.That(_patentValidationResult.Errors.Any(x => x.PropertyName == "Inventors"));
        }

        [Test]
        public void ValidatePatent_InventorsIsNull_InventorsCannotBeNull()
        {
            _patent.Inventors = null;
            var validationResult = _validator.Validate(_patent);

            Assert.That(validationResult.Errors.Any(x => x.PropertyName == "Inventors"));
        }

        [Test]
        public void ValidatePatent_CountryIsEmpty_CountryCannotBeEmpty()
        {
            Assert.That(_patentValidationResult.Errors.Any(x => x.PropertyName == "Country"));
        }

        [Test]
        public void ValidatePatent_CountryIsNull_CountryCannotBeNull()
        {
            _patent.Country = null;

            var validationResult = _validator.Validate(_patent);

            Assert.That(validationResult.Errors.Any(x => x.PropertyName == "Country"));
        }

        [Test]
        public void ValidatePatent_FillingDateIsEmpty_FillingDateCannotBeEmpty()
        {
            Assert.That(_patentValidationResult.Errors.Any(x => x.PropertyName == "FilingDate"));
        }

        [Test]
        public void ValidatePatent_FillingDateIs1949_FillingDateCannotBeLessThan1950()
        {
            _patent.FilingDate = new DateTime(1949, 1, 1);

            var validationResult = _validator.Validate(_patent);

            Assert.That(validationResult.Errors.Any(x => x.PropertyName == "FilingDate"));
        }

        [Test]
        public void ValidatePatent_PublishYearIsEmpty_PublishYearCannotBeEmpty()
        {
            Assert.That(_patentValidationResult.Errors.Any(x => x.PropertyName == "PublishYear"));
        }

        [Test]
        public void ValidatePatent_PublishYearIs1949_PublishYearCannotBeLessThan1950()
        {
            _patent.PublishYear = 1949;

            var validationResult = _validator.Validate(_patent);

            Assert.That(validationResult.Errors.Any(x => x.PropertyName == "PublishYear"));
        }

        [Test]
        public void ValidatePatent_PageCountLessThan1_PageCountCannotBeLessThan1()
        {
            Assert.That(_patentValidationResult.Errors.Any(x => x.PropertyName == "PageCount"));
        }

        [Test]
        public void ValidatePatent_NotLengthis501_NotLengthCannotBeGreaterThan500()
        {
            Assert.That(_patentValidationResult.Errors.Any(x => x.PropertyName == "Note"));
        }

        [Test]
        public void ValidatePatent_PriceIsEmpty_PriceCannotBeEmpty()
        {
            Assert.That(_patentValidationResult.Errors.Any(x => x.PropertyName == "Price"));
        }

        [Test]
        public void ValidatePatent_PriceIsNegative_PriceCannotBeNegative()
        {
            _patent.Price = -1;

            var validationResult = _validator.Validate(_patent);

            Assert.That(validationResult.Errors.Any(x => x.PropertyName == "Price"));
        }

        [TestCase("RE012345", false)]
        [TestCase("RE01234", true)]
        [TestCase("Ro012345", true)]
        [TestCase("RE1234500", true)]
        [TestCase("RE012345-1980/245", false)]
        [TestCase("RE012345-1980-245", true)]
        [TestCase("RE012345-1870/1", true)]
        [TestCase("RE012345-2100/1", true)]
        [TestCase("RE012345-20/1", true)]
        public void ValidatePatent_RegistrationNumber(string number, bool expected)
        {
            _patent.RegistrationNumber = number;

            var validationResult = _validator.Validate(_patent);
            var actual = validationResult.Errors.Any(x => x.PropertyName == "RegistrationNumber");

            Assert.AreEqual(expected, actual);
        }
    }
}
