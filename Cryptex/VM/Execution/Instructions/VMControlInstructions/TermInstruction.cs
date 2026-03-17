using Cryptex.Exceptions;
using Cryptex.VM.Execution.OperationCodes;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.VMControlInstructions;

internal sealed class TermInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Term;
    public int ScriptVersion { get; }

    internal TermInstruction(int scriptVersion) => ScriptVersion = scriptVersion;

    public void Execute(ScriptInstruction c, Executor vm)
        => throw new TerminateInstructionFoundException();
}