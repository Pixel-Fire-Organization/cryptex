using System.Globalization;

using Cryptex.Exceptions;
using Cryptex.VM.Execution.DataTypes;

namespace Cryptex.VM.Execution.Instructions.MemoryInstructions;

internal sealed class LoadInstruction : IInstruction
{

    public OpCodes OpCode => OpCodes.Load;

    public void Execute(ScriptChunkOpCode c, Executor vm)
    {
        if (c.Code != OpCode)
            throw new VMRuntimeException(ErrorCodes.VM2001_WrongOpCodePassedForScriptOpCode);

        string[] args = CryptexDataConverter.SplitInstructionArguments(c.Args, 2);

        //ARG1

        string argument1 = args[0];
        if (!argument1.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX))
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        int location1 = CryptexDataConverter.ParseArgumentToMemoryLocation(argument1);

        //ARG2

        string argument2 = args[1];
        if (argument2.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX)) //memory address => copy from $(arg2) to $(arg1)
            SwapMemoryLocation(vm.GetMemory(), argument2, location1);
        else if (argument2.StartsWith(IInstruction.DECIMAL_VALUE_PREFIX))
            SetLocationDecimal(vm.GetMemory(), argument2, location1);
        else if (argument2.StartsWith(IInstruction.HEX_VALUE_PREFIX))
            SetLocationHex(vm.GetMemory(), argument2, location1);
    }

    private static void SetLocationHex(ExecutorMemory memory, string argument2, int location1)
    {
        string arg2 = argument2.Remove(0, 1);
        if (CryptexDataConverter.IsFloatingNumber(arg2))
            throw new VMRuntimeException(ErrorCodes.VM2010_HexArgumentCannotBeAFloatingPointNumber);

        if (!CryptexDataConverter.IsIntegerNumber(arg2, NumberStyles.HexNumber))
            throw new VMRuntimeException(ErrorCodes.VM2005_DecimalArgumentIsNotANumber);

        memory.SetSlot(location1, CryptexDataConverter.
                                  GetIntegerNumber(arg2, NumberStyles.HexNumber).
                                  ToString(CultureInfo.InvariantCulture));
    }

    private static void SetLocationDecimal(ExecutorMemory memory, string argument2, int location1)
    {
        string arg2 = argument2.Remove(0, 1);
        if (!CryptexDataConverter.IsIntegerNumber(arg2) && !CryptexDataConverter.IsFloatingNumber(arg2))
            throw new VMRuntimeException(ErrorCodes.VM2005_DecimalArgumentIsNotANumber);

        memory.SetSlot(location1, CryptexDataConverter.IsIntegerNumber(arg2)
                                      ? CryptexDataConverter.GetIntegerNumber(arg2).ToString(CultureInfo.InvariantCulture)
                                      : CryptexDataConverter.GetFloatingNumber(arg2).ToString(CultureInfo.InvariantCulture));
    }

    private static void SwapMemoryLocation(ExecutorMemory memory, string argument2, int location1)
    {
        if (!int.TryParse(argument2.Remove(0, 1), out int location2))
            throw new VMRuntimeException(ErrorCodes.VM2004_MemoryArgumentIsNotANumber);

        string? value = memory.GetSlot(location2);
        if (value is null)
            throw new VMRuntimeException(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument);

        memory.SetSlot(location1, value!);
    }
}
