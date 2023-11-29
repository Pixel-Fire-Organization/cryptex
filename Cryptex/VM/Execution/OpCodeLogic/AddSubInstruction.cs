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

        string[] args = c.Args.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (args.Length != 2) // this instruction takes only 2 arguments!
            ErrorList.WriteError(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction, fatal: true);

        //ARG1

        string argument1 = args[0];
        if (!argument1.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX))
            ErrorList.WriteError(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction, fatal: true);

        if (!int.TryParse(argument1.Remove(0, 1), out int location1))
            ErrorList.WriteError(ErrorCodes.VM2004_MemoryArgumentIsNotANumber, fatal: true);

        string? slotValue1 = memory.GetSlot(location1);
        if (!CryptexDataConverter.IsIntegerNumber(slotValue1) && !CryptexDataConverter.IsDecimalNumber(slotValue1))
            ErrorList.WriteError(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument, fatal: true);

        //ARG2

        string argument2 = args[1];
        if (!argument2.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX))
            ErrorList.WriteError(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction, fatal: true);

        if (!int.TryParse(argument2.Remove(0, 1), out int location2))
            ErrorList.WriteError(ErrorCodes.VM2004_MemoryArgumentIsNotANumber, fatal: true);

        string? slotValue2 = memory.GetSlot(location2);
        if (!CryptexDataConverter.IsIntegerNumber(slotValue2) && !CryptexDataConverter.IsDecimalNumber(slotValue2))
            ErrorList.WriteError(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument, fatal: true);

        if ((CryptexDataConverter.IsIntegerNumber(slotValue1) && CryptexDataConverter.IsDecimalNumber(slotValue2)) ||
            (CryptexDataConverter.IsDecimalNumber(slotValue1) && CryptexDataConverter.IsIntegerNumber(slotValue2)))
            ErrorList.WriteError(ErrorCodes.VM2009_ArgumentsWithMismatchedTypesSpecified, fatal: true);

        string result = m_expectedType == ExpectedType.Integer
                            ? CalculateInteger(slotValue1!, slotValue2!)
                            : CalculateFloating(slotValue1!, slotValue2!);

        memory.SetSlot(location1, result);

        return null;
    }

    private string CalculateFloating(string slotValue1, string slotValue2)
    {
        decimal vd1 = 0, vd2 = 0;

        if (CryptexDataConverter.IsDecimalNumber(slotValue1))
            vd1 = CryptexDataConverter.GetDecimalNumber(slotValue1);
        if (CryptexDataConverter.IsDecimalNumber(slotValue2))
            vd2 = CryptexDataConverter.GetDecimalNumber(slotValue2);

        return m_function == InstructionFunction.Add
                   ? (vd1 + vd2).ToString(CultureInfo.InvariantCulture)
                   : (vd1 - vd2).ToString(CultureInfo.InvariantCulture);
    }

    private string CalculateInteger(string slotValue1, string slotValue2)
    {
        BigInteger vd1 = 0, vd2 = 0;

        if (CryptexDataConverter.IsIntegerNumber(slotValue1))
            vd1 = CryptexDataConverter.GetIntegerNumber(slotValue1);
        if (CryptexDataConverter.IsIntegerNumber(slotValue2))
            vd2 = CryptexDataConverter.GetIntegerNumber(slotValue2);

        return m_function == InstructionFunction.Add
                   ? (vd1 + vd2).ToString(CultureInfo.InvariantCulture)
                   : (vd1 - vd2).ToString(CultureInfo.InvariantCulture);
    }
}
