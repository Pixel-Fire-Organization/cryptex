namespace Cryptex.Exceptions;

public sealed class RequiredMessageNotFoundException : Exception
{
    public RequiredMessageNotFoundException(ErrorCodes code)
        : base($"Message for error code: `{code.ToString()}` is not found!")
    {
    }

    public RequiredMessageNotFoundException(ErrorCodes code, Exception inner)
        : base($"Message for error code: `{code.ToString()}` is not found!", inner)
    {
    }
}