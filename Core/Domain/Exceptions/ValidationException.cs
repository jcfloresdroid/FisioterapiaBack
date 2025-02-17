using FluentValidation.Results;

namespace Application.Core.Domain.Exceptions;

public class ValidationException : Exception
{
    public ValidationException() : base("Uno o más errores de validación han ocurrido.") {
        Errores = new Dictionary<string, string[]>();
    }
    
    public ValidationException(string message) : base(message) {
        Errores = null;
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this() {
        Errores = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }
    
    public IDictionary<string, string[]>? Errores { get; }
}