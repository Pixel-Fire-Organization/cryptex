using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.MathInstructions;

internal sealed class ModImmInstruction : IInstruction
{

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 2)
            throw new VmRuntimeException(ErrorCodes.Vm2002IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress ||
            c.Args[1].Type != InstructionArgumentType.Constant)
            throw new VmRuntimeException(ErrorCodes.Vm2003InvalidArgumentTypeSpecifiedForInstruction);

        var aVal = vm.GetMemory().GetSlot(c.Args[0].Value);
        var xVal = vm.GetConstant(c.Args[1].Value);

        if (!aVal.IsInteger || !xVal.IsInteger)
            throw new VmRuntimeException(ErrorCodes.Vm2011InvalidDataTypeAtSpecifiedLocation);

        if (xVal.AsInteger().IsZero)
            throw new VmRuntimeException(ErrorCodes.Vm2015DivisionByZero);

        vm.GetMemory().SetSlot(c.Args[0].Value, VmValue.FromInteger(aVal.AsInteger() % xVal.AsInteger()));
    }
}

