using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.BitwiseInstructions;

/// <summary>
///     <c>shl $A, #N</c> — shifts the integer at slot <c>$A</c> left by <c>N</c> bits.
///     <para>
///         <c>N</c> must be a decimal-integer constant (<c>#N</c>) in the range [0, <see cref="int.MaxValue" />].
///         The value at <c>$A</c> must be an integer.
///     </para>
/// </summary>
internal sealed class ShlInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Shl;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 2)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        if (c.Args[1].Type != InstructionArgumentType.Constant)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var memory = vm.GetMemory();
        var val = memory.GetSlot(c.Args[0].Value);

        if (val.IsUndefined)
            throw new VMRuntimeException(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument);

        if (!val.IsInteger)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        var shiftVal = vm.GetConstantOrThrow(in c, c.Args[1].Value);

        if (!shiftVal.IsInteger)
            throw new VMRuntimeException(ErrorCodes.VM2005_DecimalArgumentIsNotANumber);

        var shiftBig = shiftVal.AsInteger();
        if (shiftBig < 0 || shiftBig > int.MaxValue)
            throw new VMRuntimeException(ErrorCodes.VM2012_InstructionArgumentIsOutOfRange);

        memory.SetSlot(c.Args[0].Value, VMValue.FromInteger(val.AsInteger() << (int)shiftBig));
    }
}