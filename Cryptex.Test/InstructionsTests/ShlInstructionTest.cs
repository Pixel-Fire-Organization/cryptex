using Cryptex.VM.Execution;

namespace Cryptex.Test.InstructionsTests;

public sealed class ShlInstructionTest
{
    [Fact]
    public void TestShl_BigShiftingFactor()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Shl, $"$1, #{(long)int.MaxValue + 1}") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());

        string? memoryValue1 = executor.GetValueInMemory(1);
        Assert.NotNull(memoryValue1);
        Assert.Equal("5", memoryValue1);
    }

    [Fact]
    public void TestShl_SmallShiftingFactor()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Shl, $"$1, #{(long)int.MinValue - 1}") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());

        string? memoryValue1 = executor.GetValueInMemory(1);
        Assert.NotNull(memoryValue1);
        Assert.Equal("5", memoryValue1);
    }

    [Fact]
    public void TestShl_CorrectShiftingFactor()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Shl, $"$1, #5") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        string? memoryValue1 = executor.GetValueInMemory(1);
        Assert.NotNull(memoryValue1);
        Assert.Equal((5 << 5).ToString(), memoryValue1);
    }

    [Fact]
    public void TestShl_FloatingShiftingFactor()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Shl, $"$1, #5.5") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());

        string? memoryValue1 = executor.GetValueInMemory(1);
        Assert.NotNull(memoryValue1);
        Assert.Equal("5", memoryValue1);
    }

    [Fact]
    public void TestShl_FloatingValue()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5.5"), new ScriptChunkOpCode(OpCodes.Shl, $"$1, #5") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());

        string? memoryValue1 = executor.GetValueInMemory(1);
        Assert.NotNull(memoryValue1);
        Assert.Equal("5.5", memoryValue1);
    }

    [Fact]
    public void TestShl_CorrectValue()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Shl, $"$1, #5") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        string? memoryValue1 = executor.GetValueInMemory(1);
        Assert.NotNull(memoryValue1);
        Assert.Equal((5 << 5).ToString(), memoryValue1);
    }
}
