using System.Globalization;
using System.Numerics;

using Cryptex.Exceptions;
using Cryptex.VM.Execution.DataTypes;

namespace Cryptex.VM.Execution.Instructions.BitwiseInstructions;

internal sealed class AndOrXorInstruction : IInstruction
{
    public OpCodes OpCode    => OpCodes.And;
    public OpCodes OpCodeOr  => OpCodes.Or;
    public OpCodes OpCodeXor => OpCodes.Xor;

    public enum Mode { And, Or, Xor }

    private readonly Mode m_mode;

    public AndOrXorInstruction(Mode mode) => m_mode = mode;

    public void Execute(ScriptChunkOpCode c, Executor vm)
    {
        switch (m_mode)
        {
            case Mode.And when c.Code != OpCode:
            case Mode.Or when c.Code != OpCodeOr:
            case Mode.Xor when c.Code != OpCodeXor:
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
        BigInteger argument2Value = 0;

        if (argument2.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX))
            argument2Value = ParseFromMemory(vm.GetMemory(), argument2);
        else if (argument2.StartsWith(IInstruction.DECIMAL_VALUE_PREFIX))
            argument2Value = ParseFromDecimalArgument(argument2.Remove(0, 1));
        else if (argument2.StartsWith(IInstruction.HEX_VALUE_PREFIX))
            argument2Value = ParseFromHexArgument(argument2.Remove(0, 1));

        if (argument2Value > int.MaxValue || argument2Value < int.MinValue)
            throw new VMRuntimeException(ErrorCodes.VM2012_InstructionArgumentIsOutOfRange);

        if (CryptexDataConverter.GetMemoryValueAsInteger(vm.GetMemory(), location1) is null)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        BigInteger valueToOperate = CryptexDataConverter.GetMemoryValueAsInteger(vm.GetMemory(), location1) ?? 0;

        switch (m_mode)
        {
            case Mode.And:
                valueToOperate &= (int)argument2Value;
                break;
            case Mode.Or:
                valueToOperate |= (int)argument2Value;
                break;
            case Mode.Xor:
                valueToOperate ^= (int)argument2Value;
                break;
        }

        vm.GetMemory().SetSlot(location1, valueToOperate.ToString());
    }

    private BigInteger ParseFromHexArgument(string value)
    {
        if (CryptexDataConverter.IsIntegerNumber(value, NumberStyles.HexNumber))
            return CryptexDataConverter.GetIntegerNumber(value, NumberStyles.HexNumber);

        throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);
    }

    private BigInteger ParseFromDecimalArgument(string value)
    {
        if (CryptexDataConverter.IsIntegerNumber(value))
            return CryptexDataConverter.GetIntegerNumber(value);

        throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);
    }

    private BigInteger ParseFromMemory(ExecutorMemory memory, string arg)
    {
        int location = CryptexDataConverter.ParseArgumentToMemoryLocation(arg);

        var num = CryptexDataConverter.GetMemoryValueAsInteger(memory, location);
        if (num is null)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        return num.Value;
    }
}
