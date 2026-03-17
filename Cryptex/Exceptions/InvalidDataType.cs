namespace Cryptex.Exceptions;

internal sealed class InvalidDataType : Exception
{
    public InvalidDataType(string message)
        : base(message)
    {
    }

    public InvalidDataType(string message, Exception inner)
        : base(message, inner)
    {
    }
}