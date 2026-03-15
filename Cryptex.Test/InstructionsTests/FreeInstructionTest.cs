using Cryptex.VM.Execution;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Test.InstructionsTests;

public sealed class FreeInstructionTest
{
    [Fact]
    public void TestFree_OnExistingAddress()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Load, "$2, #6"), new ScriptInstruction(OpCodes.Free, "$1") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        VMValue memoryValue1 = executor.GetValueInMemory(1);
        VMValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.True(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VMValue.FromInteger(6), memoryValue2);
    }
    
    [Fact]
    public void TestFree_OnNonExistingAddress()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Load, "$2, #6"), new ScriptInstruction(OpCodes.Free, "$3") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());

        VMValue memoryValue1 = executor.GetValueInMemory(1);
        VMValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VMValue.FromInteger(6), memoryValue2);
    }
}
