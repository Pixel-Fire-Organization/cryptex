using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.MemoryInstructions;

internal sealed class FreeInstruction : IInstruction
{

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 1)
            throw new VmRuntimeException(ErrorCodes.Vm2002IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress)
            throw new VmRuntimeException(ErrorCodes.Vm2003InvalidArgumentTypeSpecifiedForInstruction);

        var removed = vm.GetMemory().RemoveSlot(c.Args[0].Value);
        if (removed.IsUndefined)
            throw new VmRuntimeException(ErrorCodes.Vm2007InvalidMemoryLocationSpecifiedAsArgument);
    }
}

