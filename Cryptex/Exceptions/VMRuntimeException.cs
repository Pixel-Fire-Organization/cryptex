namespace Cryptex.Exceptions;

// ReSharper disable once UnusedMember.Global
internal sealed class VmRuntimeException : Exception
{
    public VmRuntimeException(ErrorCodes code)
        : base($"Runtime exception: {(int)code} | {code.ToMessage()}")
    {
    }

    // ReSharper disable once UnusedMember.Global
    public VmRuntimeException(ErrorCodes code, Exception? innerException)
        : base($"Runtime exception: {(int)code} | {code.ToMessage()}", innerException)
    {
    }
}