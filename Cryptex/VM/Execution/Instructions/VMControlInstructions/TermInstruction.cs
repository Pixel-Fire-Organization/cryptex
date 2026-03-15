using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.VMControlInstructions;

/// <summary>
///     <c>term</c> — emergency VM termination sentinel.
///     This instruction must never appear in valid user scripts; it is used as a
///     fail-safe when a chunk contains invalid or unrecoverable data.
/// </summary>
internal sealed class TermInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Term;

    public void Execute(ScriptInstruction c, Executor vm) => throw new TerminateInstructionFoundException();
}