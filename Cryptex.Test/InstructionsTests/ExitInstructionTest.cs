using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.Test.InstructionsTests;

public sealed class ExitInstructionTest
{
    [Fact]
    public void TestExit_IntegerExitCode()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Exit, [Args.Const(0)])
        ]);
        Script script = new Script("script", [mainChunk], [VmValue.FromInteger(0)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());
        Assert.Equal(0, executor.GetExitCode());
    }

    [Fact]
    public void TestExit_NonZeroExitCode()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Exit, [Args.Const(0)])
        ]);
        Script script = new Script("script", [mainChunk], [VmValue.FromInteger(0x7f)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());
        Assert.Equal(0x7f, executor.GetExitCode());
    }

    [Fact]
    public void TestExit_ExitCodeInMemory()
    {
        // MemoryAddress type — Exit only accepts Constant, so it fails.
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Exit, [Args.Mem(25)])
        ]);
        Script script = new Script("script", [mainChunk]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());
        Assert.Equal(-1, executor.GetExitCode());
    }
}
