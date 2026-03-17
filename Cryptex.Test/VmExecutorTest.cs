using Cryptex.VM.Execution;
using Cryptex.VM.Execution.OperationCodes;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Test;

public sealed class VmExecutorTest
{
    // Constants: [0]=5 (int), [1]=6 (int), [2]=6.25 (float)
    private static readonly VMValue[] Constants =
        [VMValue.FromInteger(5), VMValue.FromInteger(6), VMValue.FromFloat(6.25m)];

    // A chunk that adds two integers — succeeds.
    private static ScriptChunk IntAddChunk() => new ScriptChunk("main", [
        new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
        new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
        new ScriptInstruction(OpCodes.Add, [Args.Mem(1), Args.Mem(2)])
    ]);

    // A chunk that tries to add int + float — fails.
    private static ScriptChunk MixedAddChunk() => new ScriptChunk("main", [
        new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
        new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(2)]),
        new ScriptInstruction(OpCodes.Add, [Args.Mem(1), Args.Mem(2)])
    ]);

    [Fact]
    public void TestBeginExecution_OnErrorInScript()
    {
        Script script = new Script("script", [MixedAddChunk()], Constants);
        Assert.False(new Executor(script).ExecuteScript());
    }

    [Fact]
    public void TestBeginExecution_OnNoErrorInScript()
    {
        Script script = new Script("script", [IntAddChunk()], Constants);
        Assert.True(new Executor(script).ExecuteScript());
    }

    [Fact]
    public void TestExecuteChunk_OnErrorInScript()
    {
        Script script = new Script("script", [MixedAddChunk()], Constants);
        Assert.False(new Executor(script).ExecuteChunk());
    }

    [Fact]
    public void TestExecuteChunk_OnNoErrorInScript()
    {
        Script script = new Script("script", [IntAddChunk()], Constants);
        Assert.True(new Executor(script).ExecuteChunk());
    }

    [Fact]
    public void TestExecuteChunk_OnNonExistentScriptChunk()
    {
        Script script = new Script("script", [IntAddChunk()], Constants);
        Assert.False(new Executor(script).ExecuteChunk("abc"));
    }

    [Fact]
    public void TestDumpMemory_FullMemory()
    {
        Script script = new Script("script", [IntAddChunk()], Constants);
        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());
        Assert.Equal("[1]: `11`\n[2]: `6`\n", executor.DumpMemory());
    }

    [Fact]
    public void TestDumpMemory_EmptyMemory()
    {
        Script script = new Script("script", [IntAddChunk()], Constants);
        Assert.Equal("<EMPTY>", new Executor(script).DumpMemory());
    }

    [Fact]
    public void IsInCompatibilityMode_WhenScriptVersionIsNewer_ReturnsTrue()
    {
        var script = new Script("compat", Executor.VM_VERSION + 1, "main",
            [IntAddChunk()], Constants);
        var executor = new Executor(script);

        Assert.True(executor.IsInCompatibilityMode);
    }

    [Fact]
    public void IsInCompatibilityMode_WhenScriptVersionMatchesCurrent_ReturnsFalse()
    {
        var script = new Script("script", [IntAddChunk()], Constants);

        Assert.False(new Executor(script).IsInCompatibilityMode);
    }

    [Fact]
    public void IsInCompatibilityMode_WhenScriptVersionIsOlder_ReturnsFalse()
    {
        var script = new Script("legacy", Executor.VM_VERSION - 1 < 1 ? 1 : Executor.VM_VERSION - 1,
            "main", [IntAddChunk()], Constants);

        Assert.False(new Executor(script).IsInCompatibilityMode);
    }

    [Fact]
    public void ExecuteScript_NewerVersionScript_SucceedsInCompatibilityMode()
    {
        var script = new Script("compat", Executor.VM_VERSION + 1, "main",
            [IntAddChunk()], Constants);
        var executor = new Executor(script);

        Assert.True(executor.ExecuteScript());
    }
}
