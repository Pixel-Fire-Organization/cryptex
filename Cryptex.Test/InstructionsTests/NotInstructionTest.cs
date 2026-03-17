using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.Test.InstructionsTests;

public sealed class NotInstructionTest
{
    [Fact]
    public void TestNot_ValidAddress()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Not, [Args.Mem(1)])
        ]);
        Script script = new Script("script", [mainChunk], [VMValue.FromInteger(5)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        VMValue memoryValue1 = executor.GetValueInMemory(1);
        Assert.False(memoryValue1.IsUndefined);
        Assert.Equal(VMValue.FromInteger(~5), memoryValue1);
    }

    [Fact]
    public void TestNot_Floating()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Not, [Args.Mem(1)])
        ]);
        Script script = new Script("script", [mainChunk], [VMValue.FromFloat(5.5m)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        VMValue memoryValue1 = executor.GetValueInMemory(1);
        Assert.False(memoryValue1.IsUndefined);
        Assert.Equal(VMValue.FromFloat(5.5m), memoryValue1);
    }
}
