using MessagePack;

namespace Cryptex.VM.Execution.Scripts;

[MessagePackObject(true)]
public struct ScriptInstruction
{
    public OpCodes Code { get; set; }
    public ScriptInstructionArgument[] Args { get; set; }

    public ScriptInstruction()
    {
        Code = OpCodes.Crash;
        Args = [ScriptInstructionArgument.DEFAULT];
    }

    public ScriptInstruction(OpCodes code)
    {
        Code = code;
        Args = [ScriptInstructionArgument.DEFAULT];
    }

    public ScriptInstruction(OpCodes code, ScriptInstructionArgument[] args)
    {
        Code = code;
        Args = args;
    }
}