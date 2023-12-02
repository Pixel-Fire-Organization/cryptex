namespace Cryptex.Exceptions;

internal sealed class VMRuntimeException : Exception
{
    public VMRuntimeException(ErrorCodes code)
        : base($"Runtime exception: {(int)code} | {code.ToMessage()}") { }

    public VMRuntimeException(ErrorCodes code, Exception? innerException)
        : base($"Runtime exception: {(int)code} | {code.ToMessage()}", innerException) { }
}
