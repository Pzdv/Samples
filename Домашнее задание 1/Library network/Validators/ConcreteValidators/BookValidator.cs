using Library_network.Models;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Library_network.Validators.ConcreteValidators
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            Include(new LibraryItemValidator());
            Include(new PrintedProductValidator());

            RuleFor(book => book.CreationCity).NotEmpty().WithMessage("Не может быть пустым");
            RuleFor(book => book.Autors).NotEmpty().WithMessage("Не может быть пустым");
            RuleFor(Book => Book.ISBN).Must(x => Regex.IsMatch(x, @"\d{3}-\d-\d{2}-\d{6}-\d"))
                                      .When(x => !string.IsNullOrEmpty(x.ISBN))
                                      .WithMessage("Не правильный формат ISBN");
        }
    }
}
