namespace Cryptex.VM.Execution.Scripts.Validation;

/// <summary>
///     A single validation finding produced by <see cref="ScriptValidator" />.
///     <see cref="ChunkName" /> and <see cref="InstructionIndex" /> are empty / -1 for script-level errors.
/// </summary>
public readonly struct ScriptValidationError
{
    internal ScriptValidationError(ScriptValidationErrorCode code, string message,
        string chunkName = "", int instructionIndex = -1)
    {
        Code = code;
        Message = message;
        ChunkName = chunkName;
        InstructionIndex = instructionIndex;
    }

    public ScriptValidationErrorCode Code { get; }
    public string Message { get; }
    public string ChunkName { get; }
    public int InstructionIndex { get; }

    public override string ToString() => $"[{Code}] {Message} (chunk '{ChunkName}', index {InstructionIndex})";
}
