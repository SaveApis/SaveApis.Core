namespace SaveApis.Core.Common.Application.Exceptions;

public class InvalidModuleException<TModule> : Exception
{
    public InvalidModuleException() : base($"Invalid Module: {typeof(TModule).Name}")
    {
    }

    public InvalidModuleException(string? message) : base(message)
    {
    }

    public InvalidModuleException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
