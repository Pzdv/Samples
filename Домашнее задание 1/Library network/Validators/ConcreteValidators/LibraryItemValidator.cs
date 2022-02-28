using FluentValidation;
using Library_network.Models;

namespace Library_network.Validators.ConcreteValidators
{
    public class LibraryItemValidator : AbstractValidator<LibraryItem>
    {
        public LibraryItemValidator()
        {
            RuleFor(li => li.Name).NotEmpty().WithMessage("Не может быть пустым");
            RuleFor(li => li.Note).MaximumLength(500).WithMessage("Максимальный размер не должен превышать 500 символов");
            RuleFor(li => li.Price).NotEmpty().WithMessage("Не может быть пустым")
                                   .Must(price => price >= 0).WithMessage("Не может быть отрицательной");
            RuleFor(li => li.PageCount).Must(pageCount => pageCount >= 1).WithMessage("Должно быть больше или равно 1");
        }
    }
}
