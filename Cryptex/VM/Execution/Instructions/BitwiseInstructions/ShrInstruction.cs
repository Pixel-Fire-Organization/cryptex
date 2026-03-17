using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.BitwiseInstructions;

internal sealed class ShrInstruction : IInstruction
{
    internal ShrInstruction() { }

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 2)
            throw new VmRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress ||
            c.Args[1].Type != InstructionArgumentType.Constant)
            throw new VmRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var val   = vm.GetMemory().GetSlot(c.Args[0].Value);
        var shift = vm.GetConstant(c.Args[1].Value);

        if (!val.IsInteger)
            throw new VmRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        if (!shift.IsInteger)
            throw new VmRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        var shiftAmount = shift.AsInteger();
        if (shiftAmount < int.MinValue || shiftAmount > int.MaxValue)
            throw new VmRuntimeException(ErrorCodes.VM2012_InstructionArgumentIsOutOfRange);

        vm.GetMemory().SetSlot(c.Args[0].Value, VmValue.FromInteger(val.AsInteger() >> (int)shiftAmount));
    }
}

