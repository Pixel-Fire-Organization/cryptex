using Cryptex.VM.Execution;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Test.InstructionsTests;

public sealed class CrashInstructionTest
{
    [Fact]
    public void TestCrash_MemoryAddress()
    {
        // MemoryAddress type — Crash only accepts Constant, so it fails.
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Crash, [Args.Mem(1)])
        ]);
        Script script = new Script("script", [mainChunk]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());
    }

    [Fact]
    public void TestCrash_HexValue()
    {
        // HexConstant type — Crash only accepts Constant, so it fails.
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Crash, [Args.HexConst(0)])
        ]);
        Script script = new Script("script", [mainChunk], [VMValue.FromInteger(0x7f)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());
    }

    [Fact]
    public void TestCrash_IntegerAddress()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Crash, [Args.Const(0)])
        ]);
        Script script = new Script("script", [mainChunk], [VMValue.FromInteger(2000)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());
    }

    [Fact]
    public void TestCrash_InvalidErrorCode()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Crash, [Args.Const(0)])
        ]);
        Script script = new Script("script", [mainChunk], [VMValue.FromInteger(80000)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());
    }
}
