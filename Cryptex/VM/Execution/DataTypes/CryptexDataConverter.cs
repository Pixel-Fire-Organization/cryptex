using System.Globalization;
using System.Numerics;

using Cryptex.Exceptions;
using Cryptex.VM.Execution.Instructions;

namespace Cryptex.VM.Execution.DataTypes;

internal static class CryptexDataConverter
{
    public static string[] SplitInstructionArguments(string arguments, int argumentLimit)
    {
        if (string.IsNullOrEmpty(arguments) && argumentLimit != 0)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);
        
        string[] args = arguments.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (args.Length != argumentLimit)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        return args;
    }

    public static bool IsIntegerNumber(string? value, NumberStyles style = NumberStyles.Integer)
    {
        try { GetIntegerNumber(value, style); } catch { return false; }
        return true;
    }

    public static bool IsFloatingNumber(string? value, NumberStyles style = NumberStyles.Float)
    {
        decimal d;
        try { d = GetFloatingNumber(value, style); } catch { return false; }
        return d.Scale > 0;
    }

    public static BigInteger GetIntegerNumber(string? value, NumberStyles style = NumberStyles.Integer)
    {
        if (string.IsNullOrEmpty(value))
            throw new InvalidDataType("Tried to parse integer but value wasn't an integer.");

        return BigInteger.Parse(value, style);
    }

    public static decimal GetFloatingNumber(string? value, NumberStyles style = NumberStyles.Float)
    {
        if (string.IsNullOrEmpty(value))
            throw new InvalidDataType("Tried to parse integer but value wasn't an integer.");

        return decimal.Parse(value, style, CultureInfo.InvariantCulture);
    }

    public static bool IsValidMemoryLocation(ExecutorMemory memory, int slot) => !string.IsNullOrEmpty(memory.GetSlot(slot));

    public static int ParseArgumentToMemoryLocation(string arg)
    {
        if (string.IsNullOrEmpty(arg) || !arg.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX) || !int.TryParse(arg.Remove(0, 1), out int location))
            throw new VMRuntimeException(ErrorCodes.VM2004_MemoryArgumentIsNotANumber);

        return location;
    }

    public static string GetMemoryValue(ExecutorMemory memory, int slot)
    {
        if (!IsValidMemoryLocation(memory, slot))
            throw new VMRuntimeException(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument);

        return memory.GetSlot(slot) ?? string.Empty;
    }

    public static BigInteger? GetMemoryValueAsInteger(ExecutorMemory memory, int slot)
    {
        string value = GetMemoryValue(memory, slot);
        if (string.IsNullOrEmpty(value))
            return null;

        return IsIntegerNumber(value) ? GetIntegerNumber(value) : null;
    }

    public static decimal? GetMemoryValueAsFloating(ExecutorMemory memory, int slot)
    {
        string value = GetMemoryValue(memory, slot);
        if (string.IsNullOrEmpty(value))
            return null;

        return IsFloatingNumber(value) ? GetFloatingNumber(value) : null;
    }

    public static DataTypes GetDataTypeAtMemoryLocation(ExecutorMemory memory, int slot)
    {
        string value = GetMemoryValue(memory, slot);
        if (string.IsNullOrEmpty(value))
            return DataTypes.Null;

        if (IsIntegerNumber(value) || IsFloatingNumber(value))
            return DataTypes.Number;

        return DataTypes.Null;
    }

    public static bool AreMemoryValuesOneTypeNumbers(ExecutorMemory memory, params int[] slots)
    {
        if (slots.Length == 0)
            return true;

        bool lastFloating = IsFloatingNumber(memory.GetSlot(slots[0]));
        for (int i = 1; i < slots.Length; i++)
        {
            if ((lastFloating && !IsFloatingNumber(memory.GetSlot(slots[i]))) ||
                (!lastFloating && IsFloatingNumber(memory.GetSlot(slots[i]))))
                return false;

            lastFloating = IsFloatingNumber(memory.GetSlot(slots[i]));
        }

        return true;
    }
}
