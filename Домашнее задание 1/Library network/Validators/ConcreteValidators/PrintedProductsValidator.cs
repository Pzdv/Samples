using FluentValidation;
using Library_network.Models;

namespace Library_network.Validators.ConcreteValidators
{
    public class PrintedProductValidator : AbstractValidator<PrintedProduct>
    {
        public PrintedProductValidator()
        {
            RuleFor(pp => pp.InstanceCount).Must(instanceCount => instanceCount >= 0).WithMessage("Не может быть отрицательным");
            RuleFor(pp => pp.PublishingHouse).NotEmpty().WithMessage("Не может быть пустым");
            RuleFor(pp => pp.PublishYear).GreaterThanOrEqualTo(1900).When(pp=>pp.PublishYear != 0)
                                         .WithMessage("У нас нет изданий до 1900 года"); 
        }
    }
}
