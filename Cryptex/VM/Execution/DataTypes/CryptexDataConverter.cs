using System.Globalization;
using System.Numerics;

using Cryptex.Exceptions;
using Cryptex.VM.Execution.OpCodeLogic;

namespace Cryptex.VM.Execution.DataTypes;

internal static class CryptexDataConverter
{
    public static string[]? SplitInstructionArguments(string arguments, int argumentLimit)
    {
        string[] args = arguments.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (args.Length != argumentLimit)
        {
            ErrorList.WriteError(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction, fatal: true);
            return null;
        }

        return args;
    }
    
    public static bool IsIntegerNumber(string? value, NumberStyles style = NumberStyles.Integer)
    {
        try { GetIntegerNumber(value, style); }
        catch { return false; }
        return true;
    }

    public static bool IsDecimalNumber(string? value, NumberStyles style = NumberStyles.Float)
    {
        decimal d;
        try { d = GetDecimalNumber(value, style); }
        catch { return false; }
        return d.Scale > 0;
    }

    public static BigInteger GetIntegerNumber(string? value, NumberStyles style = NumberStyles.Integer)
    {
        if (string.IsNullOrEmpty(value))
            throw new InvalidDataType("Tried to parse integer but value wasn't an integer.");

        return BigInteger.Parse(value, style);
    }

    public static decimal GetDecimalNumber(string? value, NumberStyles style = NumberStyles.Float)
    {
        if (string.IsNullOrEmpty(value))
            throw new InvalidDataType("Tried to parse integer but value wasn't an integer.");

        return decimal.Parse(value, style, CultureInfo.InvariantCulture);
    }

    public static bool IsValidMemoryLocation(ExecutorMemory memory, int slot) => !string.IsNullOrEmpty(memory.GetSlot(slot));

    public static int ParseArgumentToMemoryLocation(string arg)
    {
        if (string.IsNullOrEmpty(arg))
        {
            ErrorList.WriteError(ErrorCodes.VM2004_MemoryArgumentIsNotANumber, fatal: true);
            return 0;
        }

        if (!arg.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX))
        {
            ErrorList.WriteError(ErrorCodes.VM2004_MemoryArgumentIsNotANumber, fatal: true);
            return 0;
        }

        string contents = arg.Remove(0, 1);
        if (!int.TryParse(contents, out int location))
        {
            ErrorList.WriteError(ErrorCodes.VM2004_MemoryArgumentIsNotANumber, fatal: true);
            return 0;
        }

        return location;
    }

    public static string GetMemoryValue(ExecutorMemory memory, int slot)
    {
        if (!IsValidMemoryLocation(memory, slot))
        {
            ErrorList.WriteError(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument, fatal: true);
            return string.Empty;
        }

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

        return IsDecimalNumber(value) ? GetDecimalNumber(value) : null;
    }

    public static bool IsValueAtMemoryLocationNumber(ExecutorMemory memory, int slot)
    {
        string value = GetMemoryValue(memory, slot);
        if (string.IsNullOrEmpty(value))
            return false;

        return IsIntegerNumber(value) || IsDecimalNumber(value);
    }

    public static bool AreMemoryValuesOneTypeNumbers(ExecutorMemory memory, params int[] slots)
    {
        if (slots.Length == 0)
            return true;

        bool lastFloating = IsDecimalNumber(memory.GetSlot(slots[0]));
        for (int i = 1; i < slots.Length; i++)
        {
            if ((lastFloating && !IsDecimalNumber(memory.GetSlot(slots[i]))) || 
                (!lastFloating && IsDecimalNumber(memory.GetSlot(slots[i]))))
                return false;

            lastFloating = IsDecimalNumber(memory.GetSlot(slots[i]));
        }

        return true;
    }
}
