using Cryptex.VM.Execution.OperationCodes;
using MessagePack;

namespace Cryptex.VM.Execution.Scripts;

[MessagePackObject(true)]
public struct ScriptInstruction
{
    public OpCodes Code { get; init; }
    public ScriptInstructionArgument[] Args { get; init; }

    public ScriptInstruction()
    {
        Code = OpCodes.Crash;
        Args = [];
    }

    public ScriptInstruction(OpCodes code)
    {
        Code = code;
        Args = [];
    }

    public ScriptInstruction(OpCodes code, ScriptInstructionArgument[] args)
    {
        Code = code;
        Args = args;
    }
}
