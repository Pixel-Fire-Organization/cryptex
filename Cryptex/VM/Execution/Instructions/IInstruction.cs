using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions;

internal interface IInstruction
{
    const string MEMORY_ADDRESS_PREFIX = "$";
    const string DECIMAL_VALUE_PREFIX  = "#";
    const string HEX_VALUE_PREFIX      = "%";

    OpCodes OpCode { get; }

    void Execute(ScriptInstruction c, Executor vm);
}
