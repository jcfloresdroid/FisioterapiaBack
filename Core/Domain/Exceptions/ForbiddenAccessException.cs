namespace Core.Domain.Exceptions;

public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base() {}
    public ForbiddenAccessException(string message) : base(message) { }
}