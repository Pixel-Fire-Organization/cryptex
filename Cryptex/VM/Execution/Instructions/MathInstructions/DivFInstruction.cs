using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.MathInstructions;

internal sealed class DivFInstruction : IInstruction
{

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 2)
            throw new VmRuntimeException(ErrorCodes.Vm2002IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress ||
            c.Args[1].Type != InstructionArgumentType.MemoryAddress)
            throw new VmRuntimeException(ErrorCodes.Vm2003InvalidArgumentTypeSpecifiedForInstruction);

        var aVal = vm.GetMemory().GetSlot(c.Args[0].Value);
        var bVal = vm.GetMemory().GetSlot(c.Args[1].Value);

        if (!aVal.IsFloat || !bVal.IsFloat)
            throw new VmRuntimeException(ErrorCodes.Vm2011InvalidDataTypeAtSpecifiedLocation);

        if (bVal.AsFloat() == 0m)
            throw new VmRuntimeException(ErrorCodes.Vm2015DivisionByZero);

        vm.GetMemory().SetSlot(c.Args[0].Value, VmValue.FromFloat(aVal.AsFloat() / bVal.AsFloat()));
    }
}

