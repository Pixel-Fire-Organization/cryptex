using Cryptex.VM.Execution;

namespace Cryptex.Test.InstructionsTests;

public sealed class SubInstructionTest
{
    [Fact]
    public void TestSub_CorrectValues()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6"), new ScriptChunkOpCode(OpCodes.Sub, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        string? memoryValue1 = executor.GetValueInMemory(1);
        string? memoryValue2 = executor.GetValueInMemory(2);
        Assert.NotNull(memoryValue1);
        Assert.NotNull(memoryValue2);
        Assert.Equal("-1", memoryValue1);
        Assert.Equal("6", memoryValue2);
    }
    
    [Fact]
    public void TestSubf_CorrectValues()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5.25"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6.25"), new ScriptChunkOpCode(OpCodes.SubF, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        string? memoryValue1 = executor.GetValueInMemory(1);
        string? memoryValue2 = executor.GetValueInMemory(2);
        Assert.NotNull(memoryValue1);
        Assert.NotNull(memoryValue2);
        Assert.Equal("-1.00", memoryValue1);
        Assert.Equal("6.25", memoryValue2);
    }
    
    [Fact]
    public void TestSub_FloatingAndInteger()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5.25"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6"), new ScriptChunkOpCode(OpCodes.Sub, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        
        string? memoryValue1 = executor.GetValueInMemory(1);
        string? memoryValue2 = executor.GetValueInMemory(2);
        Assert.NotNull(memoryValue1);
        Assert.NotNull(memoryValue2);
        Assert.Equal("5.25", memoryValue1);
        Assert.Equal("6", memoryValue2);
    }
    
    [Fact]
    public void TestSubf_FloatingInteger()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5.25"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6"), new ScriptChunkOpCode(OpCodes.SubF, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        
        string? memoryValue1 = executor.GetValueInMemory(1);
        string? memoryValue2 = executor.GetValueInMemory(2);
        Assert.NotNull(memoryValue1);
        Assert.NotNull(memoryValue2);
        Assert.Equal("5.25", memoryValue1);
        Assert.Equal("6", memoryValue2);
    }
    
    [Fact]
    public void TestSub_ArgumentNotMemory()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6"), new ScriptChunkOpCode(OpCodes.Sub, "$1, #2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        
        string? memoryValue1 = executor.GetValueInMemory(1);
        string? memoryValue2 = executor.GetValueInMemory(2);
        Assert.NotNull(memoryValue1);
        Assert.NotNull(memoryValue2);
        Assert.Equal("5", memoryValue1);
        Assert.Equal("6", memoryValue2);
    }
    
    [Fact]
    public void TestSubf_ArgumentNotMemory()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5.25"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6.25"), new ScriptChunkOpCode(OpCodes.SubF, "$1, #2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        
        string? memoryValue1 = executor.GetValueInMemory(1);
        string? memoryValue2 = executor.GetValueInMemory(2);
        Assert.NotNull(memoryValue1);
        Assert.NotNull(memoryValue2);
        Assert.Equal("5.25", memoryValue1);
        Assert.Equal("6.25", memoryValue2);
    }
    
    [Fact]
    public void TestSub_TooFewArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6"), new ScriptChunkOpCode(OpCodes.Sub, "$1") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        
        string? memoryValue1 = executor.GetValueInMemory(1);
        string? memoryValue2 = executor.GetValueInMemory(2);
        Assert.NotNull(memoryValue1);
        Assert.NotNull(memoryValue2);
        Assert.Equal("5", memoryValue1);
        Assert.Equal("6", memoryValue2);
    }
    
    [Fact]
    public void TestSubf_TooFewArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5.25"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6.25"), new ScriptChunkOpCode(OpCodes.SubF, "$1") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        
        string? memoryValue1 = executor.GetValueInMemory(1);
        string? memoryValue2 = executor.GetValueInMemory(2);
        Assert.NotNull(memoryValue1);
        Assert.NotNull(memoryValue2);
        Assert.Equal("5.25", memoryValue1);
        Assert.Equal("6.25", memoryValue2);
    }
    
    [Fact]
    public void TestSub_TooMuchArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6"), new ScriptChunkOpCode(OpCodes.Sub, "$1, $2, $3") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        
        string? memoryValue1 = executor.GetValueInMemory(1);
        string? memoryValue2 = executor.GetValueInMemory(2);
        Assert.NotNull(memoryValue1);
        Assert.NotNull(memoryValue2);
        Assert.Equal("5", memoryValue1);
        Assert.Equal("6", memoryValue2);
    }
    
    [Fact]
    public void TestSubf_TooMuchArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5.25"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6.25"), new ScriptChunkOpCode(OpCodes.SubF, "$1, $2, $3") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        
        string? memoryValue1 = executor.GetValueInMemory(1);
        string? memoryValue2 = executor.GetValueInMemory(2);
        Assert.NotNull(memoryValue1);
        Assert.NotNull(memoryValue2);
        Assert.Equal("5.25", memoryValue1);
        Assert.Equal("6.25", memoryValue2);
    }
    
    [Fact]
    public void TestSub_NoArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6"), new ScriptChunkOpCode(OpCodes.Sub, "") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        
        string? memoryValue1 = executor.GetValueInMemory(1);
        string? memoryValue2 = executor.GetValueInMemory(2);
        Assert.NotNull(memoryValue1);
        Assert.NotNull(memoryValue2);
        Assert.Equal("5", memoryValue1);
        Assert.Equal("6", memoryValue2);
    }
    
    [Fact]
    public void TestSubf_NoArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5.25"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6.25"), new ScriptChunkOpCode(OpCodes.SubF, "") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        
        string? memoryValue1 = executor.GetValueInMemory(1);
        string? memoryValue2 = executor.GetValueInMemory(2);
        Assert.NotNull(memoryValue1);
        Assert.NotNull(memoryValue2);
        Assert.Equal("5.25", memoryValue1);
        Assert.Equal("6.25", memoryValue2);
    }
}
