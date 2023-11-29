using System.Globalization;
using System.Numerics;

using Cryptex.VM.Execution.DataTypes;

namespace Cryptex.VM.Execution.OpCodeLogic;

internal sealed class AddSubInstruction : IInstruction
{
    public OpCodes OpCode     => OpCodes.Add;
    public OpCodes OpCodeSub  => OpCodes.Sub;
    public OpCodes OpCodeAddD => OpCodes.AddD;
    public OpCodes OpCodeSubD => OpCodes.SubD;

    public enum InstructionFunction { Add, Subtract }

    public enum ExpectedType { Integer, Floating }

    private readonly InstructionFunction m_function;
    private readonly ExpectedType        m_expectedType;

    public AddSubInstruction(InstructionFunction f, ExpectedType expectedType)
    {
        m_function     = f;
        m_expectedType = expectedType;
    }

    public object? Execute(ScriptChunkOpCode c, ExecutorMemory memory)
    {
        switch (m_function)
        {
            case InstructionFunction.Add when (c.Code != OpCode && c.Code != OpCodeAddD):
            case InstructionFunction.Subtract when (c.Code != OpCodeSub && c.Code != OpCodeSubD):
                ErrorList.WriteError(ErrorCodes.VM2001_WrongOpCodePassedForScriptOpCode, fatal: true);
                break;
        }

        var args = CryptexDataConverter.SplitInstructionArguments(c.Args, 2);
        if (args is null)
            return null;

        //ARG1

        string argument1 = args[0];
        if (!argument1.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX))
            ErrorList.WriteError(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction, fatal: true);

        int location1 = CryptexDataConverter.ParseArgumentToMemoryLocation(argument1);
        if (!CryptexDataConverter.IsValidMemoryLocation(memory, location1))
            ErrorList.WriteError(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument, fatal: true);

        if (!CryptexDataConverter.IsValueAtMemoryLocationNumber(memory, location1))
            ErrorList.WriteError(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument, fatal: true);

        //ARG2

        string argument2 = args[1];
        if (!argument2.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX))
            ErrorList.WriteError(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction, fatal: true);

        int location2 = CryptexDataConverter.ParseArgumentToMemoryLocation(argument2);
        if (!CryptexDataConverter.IsValidMemoryLocation(memory, location2))
            ErrorList.WriteError(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument, fatal: true);

        if (!CryptexDataConverter.IsValueAtMemoryLocationNumber(memory, location2))
            ErrorList.WriteError(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument, fatal: true);

        //Check for mismatched arguments. INT+INT or FLT+FLT allowed!

        if (!CryptexDataConverter.AreMemoryValuesOneTypeNumbers(memory, location1, location2))
            ErrorList.WriteError(ErrorCodes.VM2009_ArgumentsWithMismatchedTypesSpecified, fatal: true);

        string result = m_expectedType == ExpectedType.Integer
                            ? CalculateInteger(memory, location1, location2)
                            : CalculateFloating(memory, location1, location2);

        memory.SetSlot(location1, result);

        return null;
    }

    private string CalculateFloating(ExecutorMemory memory, int slot1, int slot2)
    {
        var vd1 = CryptexDataConverter.GetMemoryValueAsFloating(memory, slot1);
        var vd2 = CryptexDataConverter.GetMemoryValueAsFloating(memory, slot2);

        if (vd1 is null || vd2 is null)
        {
            ErrorList.WriteError(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation, fatal: true);
            return string.Empty;
        }

        return m_function == InstructionFunction.Add
                   ? (vd1.Value + vd2.Value).ToString(CultureInfo.InvariantCulture)
                   : (vd1.Value - vd2.Value).ToString(CultureInfo.InvariantCulture);
    }

    private string CalculateInteger(ExecutorMemory memory, int slot1, int slot2)
    {
        var vi1 = CryptexDataConverter.GetMemoryValueAsInteger(memory, slot1);
        var vi2 = CryptexDataConverter.GetMemoryValueAsInteger(memory, slot2);

        if (vi1 is null || vi2 is null)
        {
            ErrorList.WriteError(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation, fatal: true);
            return string.Empty;
        }

        return m_function == InstructionFunction.Add
                   ? (vi1.Value + vi2.Value).ToString(CultureInfo.InvariantCulture)
                   : (vi1.Value - vi2.Value).ToString(CultureInfo.InvariantCulture);
    }
}
