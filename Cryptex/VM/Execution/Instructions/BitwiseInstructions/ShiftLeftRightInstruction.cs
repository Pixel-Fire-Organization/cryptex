using System.Globalization;
using System.Numerics;

using Cryptex.Exceptions;
using Cryptex.VM.Execution.DataTypes;

namespace Cryptex.VM.Execution.Instructions.BitwiseInstructions;

internal sealed class ShiftLeftRightInstruction : IInstruction
{
    public OpCodes OpCode           => OpCodes.Shl;
    public OpCodes OpCodeShiftRight => OpCodes.Shr;

    public enum Orientation { Left, Right }

    private readonly Orientation m_shiftingOrientation;

    public ShiftLeftRightInstruction(Orientation orientation) => m_shiftingOrientation = orientation;

    public void Execute(ScriptChunkOpCode c, Executor vm)
    {
        switch (m_shiftingOrientation)
        {
            case Orientation.Left when c.Code != OpCode:
            case Orientation.Right when (c.Code != OpCodeShiftRight):
                throw new VMRuntimeException(ErrorCodes.VM2001_WrongOpCodePassedForScriptOpCode);
        }

        var args = CryptexDataConverter.SplitInstructionArguments(c.Args, 2);

        //ARG1

        string argument1 = args[0];
        if (!argument1.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX))
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        int location1 = CryptexDataConverter.ParseArgumentToMemoryLocation(argument1);
        if (!CryptexDataConverter.IsValidMemoryLocation(vm.GetMemory(), location1) ||
            CryptexDataConverter.GetDataTypeAtMemoryLocation(vm.GetMemory(), location1) != DataTypes.DataTypes.Number)
            throw new VMRuntimeException(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument);

        //ARG2

        string argument2 = args[1];
        if (argument2.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX)) //must be a number. (amount of shift)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var shiftingFactor = ParseShiftingFactor(argument2);

        if (shiftingFactor > int.MaxValue || shiftingFactor < int.MinValue)
            throw new VMRuntimeException(ErrorCodes.VM2012_InstructionArgumentIsOutOfRange);

        if (CryptexDataConverter.GetMemoryValueAsInteger(vm.GetMemory(), location1) is null)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        BigInteger valueToShift = CryptexDataConverter.GetMemoryValueAsInteger(vm.GetMemory(), location1) ?? 0;

        if (m_shiftingOrientation == Orientation.Left)
            valueToShift <<= (int)shiftingFactor;
        if (m_shiftingOrientation == Orientation.Right)
            valueToShift >>= (int)shiftingFactor;

        vm.GetMemory().SetSlot(location1, valueToShift.ToString());
    }

    private static BigInteger ParseShiftingFactor(string argument2)
    {
        string arg2 = argument2.Remove(0, 1);
        if (!CryptexDataConverter.IsIntegerNumber(arg2) && !CryptexDataConverter.IsFloatingNumber(arg2))
            throw new VMRuntimeException(ErrorCodes.VM2005_DecimalArgumentIsNotANumber);

        if (CryptexDataConverter.IsFloatingNumber(arg2)) //only integers[-2^32; (2^32)-1] can be used as shifting factors.
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        if (argument2.StartsWith(IInstruction.DECIMAL_VALUE_PREFIX))
            return CryptexDataConverter.GetIntegerNumber(arg2);
        if (argument2.StartsWith(IInstruction.HEX_VALUE_PREFIX))
            return CryptexDataConverter.GetIntegerNumber(arg2, NumberStyles.HexNumber);

        return 0;
    }
}
