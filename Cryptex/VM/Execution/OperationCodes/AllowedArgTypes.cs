namespace Cryptex.VM.Execution.OperationCodes;

[Flags]
internal enum AllowedArgTypes
{
    None          = 0,
    Constant      = 1 << 0,
    MemoryAddress = 1 << 1,
    Label         = 1 << 2,
}

