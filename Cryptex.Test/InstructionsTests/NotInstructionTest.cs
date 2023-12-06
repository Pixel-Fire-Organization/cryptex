using Cryptex.VM.Execution;

namespace Cryptex.Test.InstructionsTests;

public sealed class NotInstructionTest
{
    [Fact]
    public void TestNot_ValidAddress()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Not, "$1") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        string? memoryValue1 = executor.GetValueInMemory(1);
        Assert.NotNull(memoryValue1);
        Assert.Equal((~5).ToString(), memoryValue1);
    }
    
    [Fact]
    public void TestNot_Floating()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5.5"), new ScriptChunkOpCode(OpCodes.Not, "$1") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());

        string? memoryValue1 = executor.GetValueInMemory(1);
        Assert.NotNull(memoryValue1);
        Assert.Equal("5.5", memoryValue1);
    }
}
