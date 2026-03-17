namespace Cryptex.Exceptions;

internal sealed class TerminateInstructionFoundException : Exception
{
    private const string MESSAGE =
        "A `term` instruction found! This indicates that the code chunk currently executing is corrupted!";

    public TerminateInstructionFoundException()
        : base(MESSAGE)
    {
    }

    // ReSharper disable once UnusedMember.Global
    public TerminateInstructionFoundException(Exception? innerException) : base(MESSAGE, innerException)
    {
    }
}