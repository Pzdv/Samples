using Library_network.Models;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Library_network.Validators.ConcreteValidators
{
    public class NewspaperValidator : AbstractValidator<Newspaper>
    {
        public NewspaperValidator()
        {
            Include(new LibraryItemValidator());
            Include(new PrintedProductValidator());

            RuleFor(newspaper => newspaper.Number).Must(number => number > 0).WithMessage("Должен быть положительный");
            RuleFor(newspaper => newspaper.Date).NotEmpty().WithMessage("Не может быть пустым");
            RuleFor(newspaper => newspaper.ISSN).Must(ISSN => Regex.IsMatch(ISSN, @"\d{4}-\d{4}"))
                                                .When(x => !string.IsNullOrEmpty(x.ISSN))
                                                .WithMessage("Неправильный формат ISSN");
        }
    }
}
