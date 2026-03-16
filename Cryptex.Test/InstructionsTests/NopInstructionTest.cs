using Cryptex.VM.Execution;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Test.InstructionsTests;

public sealed class NopInstructionTest
{
    [Fact]
    public void TestNop_FloatingTime()
    {
        // Float constant — Nop requires an integer, so it fails.
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Nop, [Args.Const(0)])
        ]);
        Script script = new Script("script", [mainChunk], [VMValue.FromFloat(5.5m)]);

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
    }

    [Fact]
    public void TestNop_HexValue()
    {
        // HexConstant type — Nop only accepts Constant, so it fails.
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Nop, [Args.HexConst(0)])
        ]);
        Script script = new Script("script", [mainChunk], [VMValue.FromInteger(5)]);

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
    }

    [Fact]
    public void TestNop_MemoryLocation()
    {
        // MemoryAddress type — Nop only accepts Constant, so it fails.
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Nop, [Args.Mem(5)])
        ]);
        Script script = new Script("script", [mainChunk]);

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
    }

    [Fact]
    public void TestNop_CorrectTime()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Nop, [Args.Const(0)])
        ]);
        Script script = new Script("script", [mainChunk], [VMValue.FromInteger(10)]);

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());
    }
}
