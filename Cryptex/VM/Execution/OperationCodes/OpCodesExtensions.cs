namespace Cryptex.VM.Execution.OperationCodes;

internal static class OpCodesExtensions
{
    /// <summary>
    ///     Returns the <see cref="OpCodeInfo" /> for this opcode, selecting the instruction
    ///     implementation appropriate for <paramref name="scriptVersion" />.
    /// </summary>
    internal static OpCodeInfo GetInfo(this OpCodes code, int scriptVersion)
        => OpCodeInfo.Get(code, scriptVersion);
}