namespace Cryptex.VM.Execution.Scripts.Validation;

public enum ScriptValidationWarningCode
{
    /// <summary>
    ///     The script targets a VM version newer than the current runtime.
    ///     The VM will execute the script in compatibility mode; instruction behaviour
    ///     may not be identical to what the target version would produce.
    /// </summary>
    CompatibilityModeActive,
}

