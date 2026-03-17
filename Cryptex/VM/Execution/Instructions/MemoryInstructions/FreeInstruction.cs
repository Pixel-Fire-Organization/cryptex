using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.MemoryInstructions;

internal sealed class FreeInstruction : IInstruction
{
    internal FreeInstruction() { }

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 1)
            throw new VmRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress)
            throw new VmRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var removed = vm.GetMemory().RemoveSlot(c.Args[0].Value);
        if (removed.IsUndefined)
            throw new VmRuntimeException(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument);
    }
}

