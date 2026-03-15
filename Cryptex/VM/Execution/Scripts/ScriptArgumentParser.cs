using System.Globalization;

namespace Cryptex.VM.Execution.Scripts;

/// <summary>
///     Parses the human-readable argument string used in the convenience
///     <see cref="ScriptInstruction(Cryptex.VM.Execution.OpCodes, string)" /> constructor into a
///     typed <see cref="ScriptInstructionArgument" /> array and an optional
///     <see cref="VMValue" />[] of inline constants.
/// </summary>
/// <remarks>
///     Constants are parsed eagerly so that repeated instruction execution incurs no parsing cost.
///     Invalid hex strings (e.g. <c>%5.5</c>) are stored as <see cref="VMValueKind.Error" /> values
///     and surfaced at execution time by <see cref="Executor.GetConstantOrThrow" />, which means
///     script construction never throws unexpectedly.
/// </remarks>
internal static class ScriptArgumentParser
{
    private const char MEMORY_PREFIX = '$';
    private const char DECIMAL_CONSTANT_PREFIX = '#';
    private const char HEX_CONSTANT_PREFIX = '%';
    private const string ARG_SEPARATOR = ", ";

    /// <summary>
    ///     Parses <paramref name="argString" /> and returns the resulting typed arguments together
    ///     with any inline constant values extracted from the string.
    /// </summary>
    internal static (ScriptInstructionArgument[] Args, VMValue[]? LocalConstants) Parse(string argString)
    {
        if (string.IsNullOrWhiteSpace(argString))
            return ([ScriptInstructionArgument.DEFAULT], null);

        var tokens = argString.Split(ARG_SEPARATOR);

        List<VMValue>? constantsList = null;
        var parsedArgs = new ScriptInstructionArgument[tokens.Length];

        for (var i = 0; i < tokens.Length; i++)
        {
            var token = tokens[i].AsSpan().Trim();

            if (token.IsEmpty)
            {
                parsedArgs[i] = ScriptInstructionArgument.DEFAULT;
                continue;
            }

            var prefix = token[0];

            if (prefix == MEMORY_PREFIX)
                parsedArgs[i] = ParseMemoryAddress(token);
            else if (prefix == DECIMAL_CONSTANT_PREFIX)
                parsedArgs[i] = ParseDecimalConstant(token, ref constantsList);
            else if (prefix == HEX_CONSTANT_PREFIX)
                parsedArgs[i] = ParseHexConstant(token, ref constantsList);
            else
                parsedArgs[i] = ParseLabel(token);
        }

        VMValue[]? constants = constantsList is not null ? [.. constantsList] : null;
        return (parsedArgs, constants);
    }

    private static ScriptInstructionArgument ParseMemoryAddress(ReadOnlySpan<char> token)
    {
        if (!int.TryParse(token[1..], NumberStyles.Integer, CultureInfo.InvariantCulture, out var addr))
            throw new ArgumentException($"Invalid memory-address argument: '{token}'");

        return new ScriptInstructionArgument(addr, InstructionArgumentType.MemoryAddress);
    }

    private static ScriptInstructionArgument ParseDecimalConstant(
        ReadOnlySpan<char> token, ref List<VMValue>? constantsList)
    {
        var text = token[1..].ToString();

        VMValue val;
        if (!VMValue.TryParse(text, out val))
            val = VMValue.FromError(ErrorCodes.VM2005_DecimalArgumentIsNotANumber);

        constantsList ??= [];
        var index = constantsList.Count;
        constantsList.Add(val);
        return new ScriptInstructionArgument(index, InstructionArgumentType.Constant);
    }

    private static ScriptInstructionArgument ParseHexConstant(
        ReadOnlySpan<char> token, ref List<VMValue>? constantsList)
    {
        var rawHex = token[1..].ToString();
        var val = VMValue.ParseHex(rawHex); // may be VMValue.Error

        constantsList ??= [];
        var index = constantsList.Count;
        constantsList.Add(val);
        return new ScriptInstructionArgument(index, InstructionArgumentType.HexConstant);
    }

    private static ScriptInstructionArgument ParseLabel(ReadOnlySpan<char> token)
    {
        if (!int.TryParse(token, NumberStyles.Integer, CultureInfo.InvariantCulture, out var label))
            throw new ArgumentException($"Invalid label argument: '{token}'");

        return new ScriptInstructionArgument(label, InstructionArgumentType.Label);
    }
}