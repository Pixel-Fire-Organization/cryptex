namespace Cryptex.VM.Execution.Scripts;

public enum InstructionArgumentType
{
    Empty = 0,
    Constant,
    MemoryAddress,
    Label,
}