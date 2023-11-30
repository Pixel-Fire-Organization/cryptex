using System.Globalization;
using System.Numerics;

using Cryptex.VM.Execution.DataTypes;

namespace Cryptex.VM.Execution.OpCodeLogic.MathInstructions;

internal sealed class IncrementDecrementInstruction : IInstruction
{
    public OpCodes OpCode     => OpCodes.Inc;
    public OpCodes OpCodeDec  => OpCodes.Dec;
    public OpCodes OpCodeIncD => OpCodes.IncD;
    public OpCodes OpCodeDecD => OpCodes.DecD;

    public enum InstructionFunction { Increment, Decrement }

    public enum ExpectedType { Integer, Floating }

    private readonly InstructionFunction m_function;
    private readonly ExpectedType        m_expectedType;

    public IncrementDecrementInstruction(InstructionFunction function, ExpectedType expectedType)
    {
        m_function     = function;
        m_expectedType = expectedType;
    }

    public object? Execute(ScriptChunkOpCode c, ExecutorMemory memory)
    {
        switch (m_function)
        {
            case InstructionFunction.Increment when (c.Code != OpCode && c.Code != OpCodeIncD):
            case InstructionFunction.Decrement when (c.Code != OpCodeDec && c.Code != OpCodeDecD):
                ErrorList.WriteError(ErrorCodes.VM2001_WrongOpCodePassedForScriptOpCode, fatal: true);
                break;
        }

        var args = CryptexDataConverter.SplitInstructionArguments(c.Args, 1);
        if (args is null)
            return null;

        string argument = args[0];
        if (!argument.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX))
            ErrorList.WriteError(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction, fatal: true);

        int location = CryptexDataConverter.ParseArgumentToMemoryLocation(argument);
        if (!CryptexDataConverter.IsValidMemoryLocation(memory, location))
            ErrorList.WriteError(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument, fatal: true);

        if (!CryptexDataConverter.IsValueAtMemoryLocationNumber(memory, location))
            ErrorList.WriteError(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument, fatal: true);

        string result = m_expectedType == ExpectedType.Integer
                            ? CalculateInteger(memory, location)
                            : CalculateDecimal(memory, location);

        memory.SetSlot(location, result);

        return null;
    }

    string CalculateInteger(ExecutorMemory memory, int slot)
    {
        BigInteger? val = CryptexDataConverter.GetMemoryValueAsInteger(memory, slot);

        if (val is null)
        {
            ErrorList.WriteError(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation, fatal: true);
            return string.Empty;
        }

        return m_function == InstructionFunction.Increment
                   ? (val.Value + 1).ToString(CultureInfo.InvariantCulture)
                   : (val.Value - 1).ToString(CultureInfo.InvariantCulture);
    }

    string CalculateDecimal(ExecutorMemory memory, int slot)
    {
        decimal? val = CryptexDataConverter.GetMemoryValueAsFloating(memory, slot);

        if (val is null)
        {
            ErrorList.WriteError(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation, fatal: true);
            return string.Empty;
        }

        return m_function == InstructionFunction.Increment
                   ? (val.Value + 1).ToString(CultureInfo.InvariantCulture)
                   : (val.Value - 1).ToString(CultureInfo.InvariantCulture);
    }
}
