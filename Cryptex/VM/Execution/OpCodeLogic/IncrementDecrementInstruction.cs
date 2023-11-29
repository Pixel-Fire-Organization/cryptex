using System.Globalization;
using System.Numerics;

using Cryptex.VM.Execution.DataTypes;

namespace Cryptex.VM.Execution.OpCodeLogic;

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

        string[] args = c.Args.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (args.Length != 1) // this instruction takes only 1 argument!
            ErrorList.WriteError(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction, fatal: true);

        string argument = args[0];
        if (!argument.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX))
            ErrorList.WriteError(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction, fatal: true);

        if (!int.TryParse(argument.Remove(0, 1), out int location))
            ErrorList.WriteError(ErrorCodes.VM2004_MemoryArgumentIsNotANumber, fatal: true);

        string? slotValue = memory.GetSlot(location);
        if (!CryptexDataConverter.IsIntegerNumber(slotValue) && !CryptexDataConverter.IsDecimalNumber(slotValue))
            ErrorList.WriteError(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument, fatal: true);

        string result = m_expectedType == ExpectedType.Integer
                            ? CalculateInteger(slotValue!)
                            : CalculateDecimal(slotValue!);

        memory.SetSlot(location, result);

        return null;
    }

    string CalculateInteger(string slotValue)
    {
        BigInteger val = CryptexDataConverter.GetIntegerNumber(slotValue);
        return m_function == InstructionFunction.Increment ? (val + 1).ToString() : (val - 1).ToString();
    }

    string CalculateDecimal(string slotValue)
    {
        decimal val = CryptexDataConverter.GetDecimalNumber(slotValue);
        return m_function == InstructionFunction.Increment
                   ? (val + 1).ToString(CultureInfo.InvariantCulture)
                   : (val - 1).ToString(CultureInfo.InvariantCulture);
    }
}
