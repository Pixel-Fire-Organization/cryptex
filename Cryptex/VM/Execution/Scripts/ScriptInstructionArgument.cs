using MessagePack;

namespace Cryptex.VM.Execution.Scripts;

[MessagePackObject(true)]
public sealed class ScriptInstructionArgument
{
    public ScriptInstructionArgument(int value, InstructionArgumentType type)
    {
        Value = value;
        Type = type;
    }

    [IgnoreMember] public static ScriptInstructionArgument DEFAULT { get; } = new(0, InstructionArgumentType.Empty);

    public int Value { get; }
    public InstructionArgumentType Type { get; }
}