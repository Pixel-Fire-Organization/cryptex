namespace Cryptex.VM.Execution.OpCodeLogic;

internal interface IInstruction
{
    public const string MEMORY_ADDRESS_PREFIX = "$";
    public const string DECIMAL_VALUE_PREFIX  = "#";
    public const string HEX_VALUE_PREFIX      = "%";

    OpCodes OpCode { get; }

    object? Execute(ScriptChunkOpCode c, ExecutorMemory memory);
}
