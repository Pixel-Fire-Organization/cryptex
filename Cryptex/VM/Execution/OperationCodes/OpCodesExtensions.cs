namespace Cryptex.VM.Execution.OperationCodes;

internal static class OpCodesExtensions
{
    internal static OpCodeInfo GetInfo(this OpCodes code)
        => OpCodeInfo.Get(code);
}