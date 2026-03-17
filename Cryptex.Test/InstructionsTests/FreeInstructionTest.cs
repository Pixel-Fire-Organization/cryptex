using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.Test.InstructionsTests;

public sealed class FreeInstructionTest
{
    [Fact]
    public void TestFree_OnExistingAddress()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Free, [Args.Mem(1)])
        ]);
        Script script = new Script("script", [mainChunk],
            [VmValue.FromInteger(5), VmValue.FromInteger(6)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        VmValue memoryValue1 = executor.GetValueInMemory(1);
        VmValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.True(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VmValue.FromInteger(6), memoryValue2);
    }

    [Fact]
    public void TestFree_OnNonExistingAddress()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Free, [Args.Mem(3)])
        ]);
        Script script = new Script("script", [mainChunk],
            [VmValue.FromInteger(5), VmValue.FromInteger(6)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        VmValue memoryValue1 = executor.GetValueInMemory(1);
        VmValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VmValue.FromInteger(6), memoryValue2);
    }
}
