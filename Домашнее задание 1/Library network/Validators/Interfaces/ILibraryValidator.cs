using FluentValidation.Results;
using Library_network.Models;
using System.Collections.Generic;

namespace Library_network.Validators.Interfaces
{
    public interface ILibraryValidator
    {
        IEnumerable<ValidationResult> Validate(IEnumerable<LibraryItem> items);
        ValidationResult Validate(LibraryItem item);
    }
}
