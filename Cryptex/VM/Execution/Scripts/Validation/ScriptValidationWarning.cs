namespace Cryptex.VM.Execution.Scripts.Validation;

/// <summary>
///     A single non-fatal finding produced by <see cref="ScriptValidator.Validate" />.
///     Warnings do not prevent execution; they inform the caller of conditions that
///     may cause behaviour differences (e.g. compatibility mode).
/// </summary>
public readonly struct ScriptValidationWarning
{
    internal ScriptValidationWarning(ScriptValidationWarningCode code, string message)
    {
        Code = code;
        Message = message;
    }

    public ScriptValidationWarningCode Code { get; }
    public string Message { get; }
}

