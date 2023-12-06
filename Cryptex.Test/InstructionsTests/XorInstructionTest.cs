using Cryptex.VM.Execution;

namespace Cryptex.Test.InstructionsTests;

public sealed class XorInstructionTest
{
    [Fact]
    public void TestXor_MemoryAddresses()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6"), new ScriptChunkOpCode(OpCodes.Xor, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        string? memoryValue1 = executor.GetValueInMemory(1);
        string? memoryValue2 = executor.GetValueInMemory(2);
        Assert.NotNull(memoryValue1);
        Assert.NotNull(memoryValue2);
        Assert.Equal((5 ^ 6).ToString(), memoryValue1);
        Assert.Equal("6", memoryValue2);
    }
    
    [Fact]
    public void TestXor_MemoryAddressAndHexValue()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Xor, "$1, %7f") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        string? memoryValue1 = executor.GetValueInMemory(1);
        Assert.NotNull(memoryValue1);
        Assert.Equal((5 ^ 0x7f).ToString(), memoryValue1);
    }
    
    [Fact]
    public void TestXor_MemoryAddressAndDecimalValue()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Xor, "$1, #10") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        string? memoryValue1 = executor.GetValueInMemory(1);
        Assert.NotNull(memoryValue1);
        Assert.Equal((5 ^ 10).ToString(), memoryValue1);
    }
    
    [Fact]
    public void TestXor_FloatingInMemoryAddresses()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5.5"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6"), new ScriptChunkOpCode(OpCodes.Xor, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());

        string? memoryValue1 = executor.GetValueInMemory(1);
        string? memoryValue2 = executor.GetValueInMemory(2);
        Assert.NotNull(memoryValue1);
        Assert.NotNull(memoryValue2);
        Assert.Equal("5.5", memoryValue1);
        Assert.Equal("6", memoryValue2);
    }
    
    [Fact]
    public void TestXor_MemoryAddresses_Floating()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6.5"), new ScriptChunkOpCode(OpCodes.Xor, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());

        string? memoryValue1 = executor.GetValueInMemory(1);
        string? memoryValue2 = executor.GetValueInMemory(2);
        Assert.NotNull(memoryValue1);
        Assert.NotNull(memoryValue2);
        Assert.Equal("5", memoryValue1);
        Assert.Equal("6.5", memoryValue2);
    }
    
    [Fact]
    public void TestXor_MemoryAddressAndHexValue_Floating()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Xor, "$1, %5.5") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());

        string? memoryValue1 = executor.GetValueInMemory(1);
        Assert.NotNull(memoryValue1);
        Assert.Equal("5", memoryValue1);
    }
    
    [Fact]
    public void TestXor_MemoryAddressAndDecimalValue_Floating()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Xor, "$1, #10.5") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());

        string? memoryValue1 = executor.GetValueInMemory(1);
        Assert.NotNull(memoryValue1);
        Assert.Equal("5", memoryValue1);
    }
    
    [Fact]
    public void TestXor_IncorrectFirstArgument()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Xor, "#1, #10") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());

        string? memoryValue1 = executor.GetValueInMemory(1);
        Assert.NotNull(memoryValue1);
        Assert.Equal("5", memoryValue1);
    }
}
