using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.VMControlInstructions;

internal sealed class TermInstruction : IInstruction
{
    public void Execute(ScriptInstruction c, Executor vm)
        => throw new TerminateInstructionFoundException();
}
