using Cryptex.VM.Execution.OperationCodes;
using Cryptex.VM.Execution.Scripts.Loaders;
using Cryptex.VM.Execution.Scripts.Validation;

namespace Cryptex.Test.LoadersTest;

public sealed class ScriptValidatorTest
{
    private static Script ValidScript() => Args.Build("valid",
        [VMValue.FromInteger(5)],
        new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
        new ScriptInstruction(OpCodes.Inc, [Args.Mem(1)]));

    private static Script EmptyChunkScript() => new Script("empty",
        [new ScriptChunk("main", [])], []);

    [Fact]
    public void Validate_ValidScript_IsValid()
    {
        var result = ScriptValidator.Validate(ValidScript());

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_EmptyChunk_IsValid()
    {
        var result = ScriptValidator.Validate(EmptyChunkScript());

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_VersionTooNew_ReportsCompatibilityModeWarning()
    {
        var script = new Script("test", Executor.VM_VERSION + 1, "main",
            [new ScriptChunk("main", [])], []);

        var result = ScriptValidator.Validate(script);

        Assert.True(result.IsValid);
        Assert.True(result.HasWarnings);
        Assert.Contains(result.Warnings, w => w.Code == ScriptValidationWarningCode.CompatibilityModeActive);
    }

    [Fact]
    public void Validate_VersionZero_ReportsInvalidVersion()
    {
        var script = new Script("test", 0, "main",
            [new ScriptChunk("main", [])], []);

        var result = ScriptValidator.Validate(script);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Code == ScriptValidationErrorCode.InvalidVersion);
    }

    [Fact]
    public void Validate_MissingEntryPointChunk_ReportsInvalidEntryPoint()
    {
        var script = new Script("test", Executor.VM_VERSION, "nonexistent",
            [new ScriptChunk("main", [])], []);

        var result = ScriptValidator.Validate(script);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Code == ScriptValidationErrorCode.InvalidEntryPoint);
    }

    [Fact]
    public void Validate_UnsupportedOpCode_ReportsUnsupportedOpCode()
    {
        var script = Args.Build("test", [],
            new ScriptInstruction(OpCodes.Arg));

        var result = ScriptValidator.Validate(script);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Code == ScriptValidationErrorCode.UnsupportedOpCode);
    }

    [Fact]
    public void Validate_InvalidOpCodeByte_ReportsInvalidOpCode()
    {
        var script = Args.Build("test", [],
            new ScriptInstruction((OpCodes)255));

        var result = ScriptValidator.Validate(script);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Code == ScriptValidationErrorCode.InvalidOpCode);
    }

    [Fact]
    public void Validate_WrongArgCount_ReportsArgumentCountMismatch()
    {
        // Add expects exactly 2 args
        var script = Args.Build("test", [],
            new ScriptInstruction(OpCodes.Add));

        var result = ScriptValidator.Validate(script);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Code == ScriptValidationErrorCode.ArgumentCountMismatch);
    }

    [Fact]
    public void Validate_WrongArgType_ReportsInvalidArgumentType()
    {
        // Inc expects MemoryAddress, but Label is supplied
        var script = Args.Build("test", [],
            new ScriptInstruction(OpCodes.Inc, [Args.Label(0)]));

        var result = ScriptValidator.Validate(script);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Code == ScriptValidationErrorCode.InvalidArgumentType);
    }

    [Fact]
    public void Validate_ConstantsIndexOutOfRange_ReportsConstantsIndexOutOfRange()
    {
        // 0 constants in block, Load references index 0
        var script = Args.Build("test", [],
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]));

        var result = ScriptValidator.Validate(script);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Code == ScriptValidationErrorCode.ConstantsIndexOutOfRange);
    }

    [Fact]
    public void Validate_MultipleInstructionErrors_AllCollected()
    {
        var script = Args.Build("test", [],
            new ScriptInstruction(OpCodes.Add),           // wrong arg count
            new ScriptInstruction(OpCodes.Inc, [Args.Label(0)])); // wrong arg type

        var result = ScriptValidator.Validate(script);

        Assert.False(result.IsValid);
        Assert.True(result.Errors.Length >= 2);
    }

    [Fact]
    public void Validate_Load_AcceptsMemoryAddressAsSecondArg()
    {
        var script = Args.Build("test", [],
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Mem(2)]));

        var result = ScriptValidator.Validate(script);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void LoadAndValidate_ValidScript_ReturnsScriptAndIsValid()
    {
        var data = ScriptLoader.Save(ValidScript());

        var script = ScriptLoader.LoadAndValidate(data, out var result);

        Assert.NotNull(script);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void LoadAndValidate_EmptyData_ReturnsNullWithCouldNotLoad()
    {
        var script = ScriptLoader.LoadAndValidate([], out var result);

        Assert.Null(script);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Code == ScriptValidationErrorCode.CouldNotLoad);
    }

    [Fact]
    public void LoadAndValidate_NonExistentPath_ReturnsNullWithCouldNotLoad()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "nonexistent.script");

        var script = ScriptLoader.LoadAndValidate(path, out var result);

        Assert.Null(script);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Code == ScriptValidationErrorCode.CouldNotLoad);
    }
}

