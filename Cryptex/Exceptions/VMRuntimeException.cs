namespace Cryptex.Exceptions;

internal sealed class VmRuntimeException : Exception
{
    public VmRuntimeException(ErrorCodes code)
        : base($"Runtime exception: {(int)code} | {code.ToMessage()}")
    {
    }
}
