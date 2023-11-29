using System.Globalization;
using System.Numerics;

using Cryptex.Exceptions;

namespace Cryptex.VM.Execution.DataTypes;

internal static class CryptexDataConverter
{
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
}
