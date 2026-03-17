using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.BitwiseInstructions;

internal sealed class ShrInstruction : IInstruction
{

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 2)
            throw new VmRuntimeException(ErrorCodes.Vm2002IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress ||
            c.Args[1].Type != InstructionArgumentType.Constant)
            throw new VmRuntimeException(ErrorCodes.Vm2003InvalidArgumentTypeSpecifiedForInstruction);

        var val   = vm.GetMemory().GetSlot(c.Args[0].Value);
        var shift = vm.GetConstant(c.Args[1].Value);

        if (!val.IsInteger)
            throw new VmRuntimeException(ErrorCodes.Vm2011InvalidDataTypeAtSpecifiedLocation);

        if (!shift.IsInteger)
            throw new VmRuntimeException(ErrorCodes.Vm2011InvalidDataTypeAtSpecifiedLocation);

        var shiftAmount = shift.AsInteger();
        if (shiftAmount < int.MinValue || shiftAmount > int.MaxValue)
            throw new VmRuntimeException(ErrorCodes.Vm2012InstructionArgumentIsOutOfRange);

        vm.GetMemory().SetSlot(c.Args[0].Value, VmValue.FromInteger(val.AsInteger() >> (int)shiftAmount));
    }
}

