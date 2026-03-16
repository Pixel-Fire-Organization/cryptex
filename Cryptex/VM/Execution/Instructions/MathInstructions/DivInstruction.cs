using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.MathInstructions;

internal sealed class DivInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Div;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 2)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress ||
            c.Args[1].Type != InstructionArgumentType.MemoryAddress)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var aVal = vm.GetMemory().GetSlot(c.Args[0].Value);
        var bVal = vm.GetMemory().GetSlot(c.Args[1].Value);

        if (!aVal.IsInteger || !bVal.IsInteger)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        if (bVal.AsInteger().IsZero)
            throw new VMRuntimeException(ErrorCodes.VM2015_DivisionByZero);

        vm.GetMemory().SetSlot(c.Args[0].Value, VMValue.FromInteger(aVal.AsInteger() / bVal.AsInteger()));
    }
}

