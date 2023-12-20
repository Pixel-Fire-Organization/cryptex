using Cryptex.Exceptions;

namespace Cryptex.VM.Execution.Instructions.VMControlInstructions;

internal sealed class TermInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Term;

    public void Execute(ScriptChunkOpCode c, Executor vm)
    {
        if (c.Code != OpCode)
            throw new VMRuntimeException(ErrorCodes.VM2001_WrongOpCodePassedForScriptOpCode);

        throw new TerminateInstructionFoundException();
    }
}
