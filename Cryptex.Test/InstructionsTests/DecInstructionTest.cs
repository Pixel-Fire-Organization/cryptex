using Cryptex.VM.Execution;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Test.InstructionsTests;

public sealed class DecInstructionTest
{
    [Fact]
    public void TestDec_CorrectValue()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Dec, [Args.Mem(1)])
        ]);
        Script script = new Script("script", [mainChunk], [VMValue.FromInteger(5)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        VMValue memoryValue = executor.GetValueInMemory(1);
        Assert.False(memoryValue.IsUndefined);
        Assert.Equal(VMValue.FromInteger(4), memoryValue);
    }

    [Fact]
    public void TestDec_Floating()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.DecF, [Args.Mem(1)])
        ]);
        Script script = new Script("script", [mainChunk], [VMValue.FromFloat(5.25m)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        VMValue memoryValue = executor.GetValueInMemory(1);
        Assert.False(memoryValue.IsUndefined);
        Assert.Equal(VMValue.FromFloat(4.25m), memoryValue);
    }
}
