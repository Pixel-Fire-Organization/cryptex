using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.VM.Execution.Scripts.Validation;

public enum ScriptValidationErrorCode
{
    /// <summary>The script's <c>VMVersion</c> field is less than 1 (no such VM version exists).</summary>
    InvalidVersion,


    /// <summary>The declared entry-point chunk does not exist in the script.</summary>
    InvalidEntryPoint,

    /// <summary>An opcode is defined in the enum but has no implementation in this VM version.</summary>
    UnsupportedOpCode,

    /// <summary>An opcode byte value falls outside the known <see cref="OpCodes" /> range.</summary>
    InvalidOpCode,

    /// <summary>An instruction has fewer or more arguments than its opcode requires.</summary>
    ArgumentCountMismatch,

    /// <summary>An argument has a type that the opcode does not accept at that position.</summary>
    InvalidArgumentType,

    /// <summary>A constant-index argument points beyond the end of the script's constants block.</summary>
    ConstantsIndexOutOfRange,

    /// <summary>The raw script data could not be deserialized.</summary>
    CouldNotLoad,
}

