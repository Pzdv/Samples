using FluentValidation.Results;
using Library_network.Models;
using Library_network.Validators.ConcreteValidators;
using Library_network.Validators.Interfaces;
using System;
using System.Collections.Generic;

namespace Library_network.Validators
{
    public class LibraryFluentValidator : ILibraryValidator
    {
        private readonly BookValidator bookValidator = new();
        private readonly NewspaperValidator newspaperValidator = new();
        private readonly PatentValidator patentValidator = new();

        public IEnumerable<ValidationResult> Validate(IEnumerable<LibraryItem> items)
        {
            foreach (var item in items)
            {
                yield return Validate(item);
            }
        }

        public ValidationResult Validate(LibraryItem item)
        {
            return item.GetType().Name switch
            {
                "Book" => ValidateBook(item as Book),
                "Newspaper" => ValidateNewspaper(item as Newspaper),
                "Patent" => ValidatePatent(item as Patent),
                _ => throw new NotImplementedException(message: $"Для типа {item.GetType().FullName} нет реализации валидации"),
            };
        }

        private ValidationResult ValidateBook(Book book)
        {
            return bookValidator.Validate(book);
        }

        private ValidationResult ValidateNewspaper(Newspaper newspaper)
        {
            return newspaperValidator.Validate(newspaper);
        }

        private ValidationResult ValidatePatent(Patent patent)
        {
            return patentValidator.Validate(patent);
        }
    }
}
