using Library_network.Models;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Library_network.Validators.ConcreteValidators
{
    public class PatentValidator : AbstractValidator<Patent>
    { 
        public PatentValidator()
        {
            Include(new LibraryItemValidator());

            RuleFor(patent => patent.Inventors).NotEmpty().WithMessage("Не может быть пустым");
            RuleFor(patent => patent.Country).NotEmpty().WithMessage("Не может быть пустым");
            RuleFor(patent => patent.PublishYear).NotEmpty().WithMessage("Не может быть пустым")
                                                 .Must(year => year >= 1950).WithMessage("Не ранее 1950 года");
            RuleFor(patent => patent.FilingDate).NotEmpty().WithMessage("Не может быть пустым")
                                                .Must(date => date.Year >= 1950).WithMessage("Не ранее 1950 года");
            RuleFor(patent => patent.RegistrationNumber).Must(x => Regex.IsMatch(x, @"^RE\d{6}$") || Regex.IsMatch(x, @"^RE\d{6}\b-(19|20)\d{2}/\d{1,}"))
                                                        .When(x => !string.IsNullOrEmpty(x.RegistrationNumber))
                                                        .WithMessage("Неправильный формат регистрационного номера");
        }
    }
}
