using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.MathInstructions;

internal sealed class DecInstruction : IInstruction
{

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 1)
            throw new VmRuntimeException(ErrorCodes.Vm2002IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress)
            throw new VmRuntimeException(ErrorCodes.Vm2003InvalidArgumentTypeSpecifiedForInstruction);

        var val = vm.GetMemory().GetSlot(c.Args[0].Value);
        if (!val.IsInteger)
            throw new VmRuntimeException(ErrorCodes.Vm2011InvalidDataTypeAtSpecifiedLocation);

        vm.GetMemory().SetSlot(c.Args[0].Value, VmValue.FromInteger(val.AsInteger() - 1));
    }
}

