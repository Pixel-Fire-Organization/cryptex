using System.Globalization;

using Cryptex.VM.Execution.DataTypes;

namespace Cryptex.VM.Execution.OpCodeLogic.MemoryInstructions;

internal sealed class LoadInstruction : IInstruction
{

    public OpCodes OpCode => OpCodes.Load;

    public object? Execute(ScriptChunkOpCode c, ExecutorMemory memory)
    {
        if (c.Code != OpCode)
            ErrorList.WriteError(ErrorCodes.VM2001_WrongOpCodePassedForScriptOpCode, fatal: true);

        string[]? args = CryptexDataConverter.SplitInstructionArguments(c.Args, 2);
        if (args is null)
            return null;

        //ARG1

        string argument1 = args[0];
        if (!argument1.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX))
            ErrorList.WriteError(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction, fatal: true);

        int location1 = CryptexDataConverter.ParseArgumentToMemoryLocation(argument1);

        //ARG2

        string argument2 = args[1];
        if (argument2.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX)) //memory address => copy from $(arg2) to $(arg1)
            SwapMemoryLocation(memory, argument2, location1);
        else if (argument2.StartsWith(IInstruction.DECIMAL_VALUE_PREFIX))
            SetLocationDecimal(memory, argument2, location1);
        else if (argument2.StartsWith(IInstruction.HEX_VALUE_PREFIX))
            SetLocationHex(memory, argument2, location1);
        
        return null;
    }

    private static void SetLocationHex(ExecutorMemory memory, string argument2, int location1)
    {
        string arg2 = argument2.Remove(0, 1);
        if (CryptexDataConverter.IsDecimalNumber(arg2))
            ErrorList.WriteError(ErrorCodes.VM2010_HexArgumentCannotBeAFloatingPointNumber, fatal: true);

        if (!CryptexDataConverter.IsIntegerNumber(arg2, NumberStyles.HexNumber))
            ErrorList.WriteError(ErrorCodes.VM2005_DecimalArgumentIsNotANumber, fatal: true);

        memory.SetSlot(location1, CryptexDataConverter.
                                  GetIntegerNumber(arg2, NumberStyles.HexNumber).
                                  ToString(CultureInfo.InvariantCulture));
    }

    private static void SetLocationDecimal(ExecutorMemory memory, string argument2, int location1)
    {
        string arg2 = argument2.Remove(0, 1);
        if (!CryptexDataConverter.IsIntegerNumber(arg2) && !CryptexDataConverter.IsDecimalNumber(arg2))
            ErrorList.WriteError(ErrorCodes.VM2005_DecimalArgumentIsNotANumber, fatal: true);

        memory.SetSlot(location1, CryptexDataConverter.IsIntegerNumber(arg2)
                                      ? CryptexDataConverter.GetIntegerNumber(arg2).ToString(CultureInfo.InvariantCulture)
                                      : CryptexDataConverter.GetDecimalNumber(arg2).ToString(CultureInfo.InvariantCulture));
    }

    private static void SwapMemoryLocation(ExecutorMemory memory, string argument2, int location1)
    {
        if (!int.TryParse(argument2.Remove(0, 1), out int location2))
            ErrorList.WriteError(ErrorCodes.VM2004_MemoryArgumentIsNotANumber, fatal: true);

        string? value = memory.GetSlot(location2);
        if (value is null)
            ErrorList.WriteError(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument, fatal: true);

        memory.SetSlot(location1, value!);
    }
}
