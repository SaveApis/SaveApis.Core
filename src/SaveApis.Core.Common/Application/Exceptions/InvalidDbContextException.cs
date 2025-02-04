namespace SaveApis.Core.Common.Application.Exceptions;

public class InvalidDbContextException<TContext> : Exception
{
    public InvalidDbContextException() : base($"Invalid DbContext: {typeof(TContext).Name}")
    {
    }

    public InvalidDbContextException(string? message) : base(message)
    {
    }

    public InvalidDbContextException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
