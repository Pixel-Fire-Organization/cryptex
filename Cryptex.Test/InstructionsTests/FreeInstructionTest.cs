using Cryptex.VM.Execution;

namespace Cryptex.Test.InstructionsTests;

public sealed class FreeInstructionTest
{
    [Fact]
    public void TestFree_OnExistingAddress()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6"), new ScriptChunkOpCode(OpCodes.Free, "$1") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        string? memoryValue1 = executor.GetValueInMemory(1);
        string? memoryValue2 = executor.GetValueInMemory(2);
        Assert.Null(memoryValue1);
        Assert.NotNull(memoryValue2);
        Assert.Equal("6", memoryValue2);
    }
    
    [Fact]
    public void TestFree_OnNonExistingAddress()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6"), new ScriptChunkOpCode(OpCodes.Free, "$3") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());

        string? memoryValue1 = executor.GetValueInMemory(1);
        string? memoryValue2 = executor.GetValueInMemory(2);
        Assert.NotNull(memoryValue1);
        Assert.NotNull(memoryValue2);
        Assert.Equal("6", memoryValue2);
    }
}
