namespace Cryptex.VM.Execution.Instructions;

internal interface IInstruction
{
    public const string MEMORY_ADDRESS_PREFIX = "$";
    public const string DECIMAL_VALUE_PREFIX  = "#";
    public const string HEX_VALUE_PREFIX      = "%";

    OpCodes OpCode { get; }

    void Execute(ScriptChunkOpCode c, Executor vm);
}
