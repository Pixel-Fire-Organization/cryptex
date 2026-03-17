using Cryptex.VM.Execution.OperationCodes;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions;

internal interface IInstruction
{
    OpCodes OpCode { get; }
    int ScriptVersion { get; }

    void Execute(ScriptInstruction c, Executor vm);
}