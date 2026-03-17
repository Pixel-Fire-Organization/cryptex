using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.VM.Execution.Scripts.Validation;

/// <summary>
///     Validates a <see cref="Script" /> without executing it.
///     Checks VM-version compatibility, entry-point presence, and per-instruction
///     opcode support, argument counts, argument types, and constant-index bounds.
///     All errors are collected; validation does not stop at the first finding.
///     Non-fatal conditions (e.g. compatibility mode) are reported as <see cref="ScriptValidationWarning" />
///     and do not cause <see cref="ScriptValidationResult.IsValid" /> to return <c>false</c>.
/// </summary>
public static class ScriptValidator
{
    public static ScriptValidationResult Validate(Script script)
    {
        var errors = new List<ScriptValidationError>();
        var warnings = new List<ScriptValidationWarning>();
        ValidateVersion(script, errors, warnings);
        ValidateEntryPoint(script, errors);
        ValidateInstructions(script, errors);
        return errors.Count == 0 && warnings.Count == 0
            ? ScriptValidationResult.Valid
            : new ScriptValidationResult(errors.ToArray(), warnings.ToArray());
    }

    private static void ValidateVersion(Script script, List<ScriptValidationError> errors,
        List<ScriptValidationWarning> warnings)
    {
        if (script.VmVersion < 1)
            errors.Add(new ScriptValidationError(
                ScriptValidationErrorCode.InvalidVersion,
                $"Script has an invalid VM version ({script.VmVersion}). Minimum supported version is 1."));
        else if (script.VmVersion > Executor.VmVersion)
            warnings.Add(new ScriptValidationWarning(
                ScriptValidationWarningCode.CompatibilityModeActive,
                $"Script targets VM version {script.VmVersion}, but the current VM version is {Executor.VmVersion}. " +
                $"The VM will execute in compatibility mode. Behaviour may not be identical to VM version {script.VmVersion}."));
    }

    private static void ValidateEntryPoint(Script script, List<ScriptValidationError> errors)
    {
        if (script.GetChunk(script.EntryPointName) is null)
            errors.Add(new ScriptValidationError(
                ScriptValidationErrorCode.InvalidEntryPoint,
                $"Entry point chunk '{script.EntryPointName}' does not exist in the script."));
    }

    private static void ValidateInstructions(Script script, List<ScriptValidationError> errors)
    {
        foreach (var chunk in script.Chunks)
        {
            for (var i = 0; i < chunk.Instructions.Length; i++)
                ValidateInstruction(chunk.Instructions[i], i, chunk.ChunkName, script, errors);
        }
    }

    private static void ValidateInstruction(ScriptInstruction instruction, int index,
        string chunkName, Script script, List<ScriptValidationError> errors)
    {
        if (!OpCodeInfo.IsKnownOpCode(instruction.Code))
        {
            errors.Add(new ScriptValidationError(
                ScriptValidationErrorCode.InvalidOpCode,
                $"Unknown opcode 0x{(byte)instruction.Code:X2} at instruction {index} in chunk '{chunkName}'.",
                chunkName, index));
            return;
        }

        var info = instruction.Code.GetInfo();
        if (!info.IsSupported)
        {
            errors.Add(new ScriptValidationError(
                ScriptValidationErrorCode.UnsupportedOpCode,
                $"Opcode '{instruction.Code}' at instruction {index} in chunk '{chunkName}' is not implemented in this VM.",
                chunkName, index));
            return;
        }

        if (info.IntroducedInVersion > script.VmVersion)
        {
            errors.Add(new ScriptValidationError(
                ScriptValidationErrorCode.UnsupportedOpCode,
                $"Opcode '{instruction.Code}' at instruction {index} in chunk '{chunkName}' was introduced in VM version {info.IntroducedInVersion}, but script targets version {script.VmVersion}.",
                chunkName, index));
            return;
        }

        if (instruction.Args.Length < info.MinArgCount || instruction.Args.Length > info.MaxArgCount)
        {
            errors.Add(new ScriptValidationError(
                ScriptValidationErrorCode.ArgumentCountMismatch,
                $"Opcode '{instruction.Code}' at instruction {index} in chunk '{chunkName}' expects {info.MinArgCount}�{info.MaxArgCount} argument(s) but received {instruction.Args.Length}.",
                chunkName, index));
            return;
        }

        for (var j = 0; j < instruction.Args.Length; j++)
        {
            var arg = instruction.Args[j];
            var allowed = info.ArgAllowedTypes[j];
            var argTypeFlag = ToAllowedArgTypes(arg.Type);
            if ((allowed & argTypeFlag) == 0)
            {
                errors.Add(new ScriptValidationError(
                    ScriptValidationErrorCode.InvalidArgumentType,
                    $"Argument {j} of '{instruction.Code}' at instruction {index} in chunk '{chunkName}' has invalid type '{arg.Type}'.",
                    chunkName, index));
                continue;
            }

            if (arg.Type is InstructionArgumentType.Constant &&
                (uint)arg.Value >= (uint)script.ConstantsBlock.Count)
                errors.Add(new ScriptValidationError(
                    ScriptValidationErrorCode.ConstantsIndexOutOfRange,
                    $"Argument {j} of '{instruction.Code}' at instruction {index} in chunk '{chunkName}' references constant index {arg.Value}, but the constants block has {script.ConstantsBlock.Count} entr{(script.ConstantsBlock.Count == 1 ? "y" : "ies")}.",
                    chunkName, index));
        }
    }

    private static AllowedArgTypes ToAllowedArgTypes(InstructionArgumentType type) => type switch
    {
        InstructionArgumentType.Constant => AllowedArgTypes.Constant,
        InstructionArgumentType.MemoryAddress => AllowedArgTypes.MemoryAddress,
        InstructionArgumentType.Label => AllowedArgTypes.Label,
        _ => AllowedArgTypes.None,
    };
}