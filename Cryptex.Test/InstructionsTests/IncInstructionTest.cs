using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.Test.InstructionsTests;

public sealed class IncInstructionTest
{
    [Fact]
    public void TestInc_CorrectValue()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Inc, [Args.Mem(1)])
        ]);
        Script script = new Script("script", [mainChunk], [VMValue.FromInteger(5)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        VMValue memoryValue = executor.GetValueInMemory(1);
        Assert.False(memoryValue.IsUndefined);
        Assert.Equal(VMValue.FromInteger(6), memoryValue);
    }

    [Fact]
    public void TestInc_Floating()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.IncF, [Args.Mem(1)])
        ]);
        Script script = new Script("script", [mainChunk], [VMValue.FromFloat(5.5m)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        VMValue memoryValue = executor.GetValueInMemory(1);
        Assert.False(memoryValue.IsUndefined);
        Assert.Equal(VMValue.FromFloat(6.5m), memoryValue);
    }
}
