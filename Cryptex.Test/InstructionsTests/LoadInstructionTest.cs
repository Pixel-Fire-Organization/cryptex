using Cryptex.VM.Execution;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Test.InstructionsTests;

public sealed class LoadInstructionTest
{
    [Fact]
    public void TestLoad_ValueToMemoryLocation()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        VMValue memoryValue = executor.GetValueInMemory(1);
        Assert.False(memoryValue.IsUndefined);
        Assert.Equal(VMValue.FromInteger(5), memoryValue);
    }

    [Fact]
    public void TestLoad_HexValueToMemoryLocation()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, %10") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        VMValue memoryValue = executor.GetValueInMemory(1);
        Assert.False(memoryValue.IsUndefined);
        Assert.Equal(VMValue.FromInteger(16), memoryValue);
    }

    [Fact]
    public void TestLoad_SetMemoryLocation1ToMemoryLocation2Value()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Load, "$2, #6"), new ScriptInstruction(OpCodes.Load, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        VMValue memoryValue1 = executor.GetValueInMemory(1);
        VMValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VMValue.FromInteger(6), memoryValue1);
        Assert.Equal(VMValue.FromInteger(6), memoryValue2);
    }
}
