using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.VMControlInstructions;

internal sealed class TermInstruction : IInstruction
{
    internal TermInstruction()
    {
    }


    public void Execute(ScriptInstruction c, Executor vm)
        => throw new TerminateInstructionFoundException();
}