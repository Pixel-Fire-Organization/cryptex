using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.Test.InstructionsTests;

public sealed class LoadInstructionTest
{
    [Fact]
    public void TestLoad_ValueToMemoryLocation()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)])
        ]);
        Script script = new Script("script", [mainChunk], [VmValue.FromInteger(5)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        VmValue memoryValue = executor.GetValueInMemory(1);
        Assert.False(memoryValue.IsUndefined);
        Assert.Equal(VmValue.FromInteger(5), memoryValue);
    }

    [Fact]
    public void TestLoad_IntegerConstantToMemoryLocation()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)])
        ]);
        Script script = new Script("script", [mainChunk], [VmValue.FromInteger(16)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        VmValue memoryValue = executor.GetValueInMemory(1);
        Assert.False(memoryValue.IsUndefined);
        Assert.Equal(VmValue.FromInteger(16), memoryValue);
    }

    [Fact]
    public void TestLoad_SetMemoryLocation1ToMemoryLocation2Value()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Mem(2)])
        ]);
        Script script = new Script("script", [mainChunk],
            [VmValue.FromInteger(5), VmValue.FromInteger(6)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        VmValue memoryValue1 = executor.GetValueInMemory(1);
        VmValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VmValue.FromInteger(6), memoryValue1);
        Assert.Equal(VmValue.FromInteger(6), memoryValue2);
    }
}
