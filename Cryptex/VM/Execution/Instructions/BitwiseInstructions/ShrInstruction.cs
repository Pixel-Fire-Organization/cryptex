using System.Numerics;
using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.BitwiseInstructions;

internal sealed class ShrInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Shr;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 2)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress ||
            c.Args[1].Type != InstructionArgumentType.Constant)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var val   = vm.GetMemory().GetSlot(c.Args[0].Value);
        var shift = vm.GetConstant(c.Args[1].Value);

        if (!val.IsInteger)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        if (!shift.IsInteger)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        var shiftAmount = shift.AsInteger();
        if (shiftAmount < int.MinValue || shiftAmount > int.MaxValue)
            throw new VMRuntimeException(ErrorCodes.VM2012_InstructionArgumentIsOutOfRange);

        vm.GetMemory().SetSlot(c.Args[0].Value, VMValue.FromInteger(val.AsInteger() >> (int)shiftAmount));
    }
}

