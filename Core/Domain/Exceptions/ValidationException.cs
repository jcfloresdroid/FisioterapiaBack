using FluentValidation.Results;

namespace Application.Core.Domain.Exceptions;

public class ValidationException : Exception
{
    public ValidationException() : base("Uno o más errores de validación han ocurrido.") {
        Error = null;
    }
    
    public ValidationException(IEnumerable<ValidationFailure> failures) : this() {
        // Toma solo el último error de validación
        Error = failures.LastOrDefault()?.ErrorMessage;
    }
    
    public string? Error { get; }
}