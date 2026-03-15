using MessagePack;

namespace Cryptex.VM.Execution.Scripts;

[MessagePackObject(keyAsPropertyName: true)]
public sealed class ScriptInstructionArgument
{
    [IgnoreMember]
    public static ScriptInstructionArgument DEFAULT { get; } = new(0, InstructionArgumentType.Empty);
    
    public int Value { get; }
    public InstructionArgumentType Type { get; }

    public ScriptInstructionArgument(int value, InstructionArgumentType type)
    {
        Value = value;
        Type = type;
    }
}