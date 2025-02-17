namespace Core.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : base() { }
    
    public NotFoundException(string message) : base(message) { }
    
    public NotFoundException(string name, object key)
        : base($"El recurso \"{name}\" ({key}) no fue encontrado.") { }
}