namespace Cryptex.VM.Execution.Scripts.Validation;

/// <summary>
///     The outcome of a <see cref="ScriptValidator.Validate" /> call.
///     <see cref="IsValid" /> is <c>true</c> when <see cref="Errors" /> is empty.
///     A result may be valid yet still carry <see cref="Warnings" /> (e.g. compatibility mode active).
/// </summary>
public readonly struct ScriptValidationResult
{
    private static readonly ScriptValidationError[] s_unloadableErrors =
    [
        new ScriptValidationError(ScriptValidationErrorCode.CouldNotLoad,
            "The script data could not be deserialized."),
    ];

    internal ScriptValidationResult(ScriptValidationError[] errors, ScriptValidationWarning[] warnings)
    {
        Errors = errors;
        Warnings = warnings;
    }

    public bool IsValid => Errors is null or { Length: 0 };
    public bool HasWarnings => Warnings is { Length: > 0 };

    public ScriptValidationError[] Errors { get; }
    public ScriptValidationWarning[] Warnings { get; }

    public static ScriptValidationResult Valid { get; } = new([], []);

    /// <summary>Returned by <c>LoadAndValidate</c> when the raw data cannot be deserialized at all.</summary>
    public static ScriptValidationResult Unloadable { get; } = new(s_unloadableErrors, []);
}



