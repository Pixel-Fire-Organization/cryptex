using Cryptex.VM.Execution;

namespace Cryptex.Test.InstructionsTests;

public sealed class LoadInstructionTest
{
    [Fact]
    public void TestVmExecute_LoadInstruction_ValueToMemoryLocation()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        executor.BeginExecution();

        string? memoryValue = executor.GetValueInMemory(1);
        Assert.NotNull(memoryValue);
        Assert.Equal("5", memoryValue);
    }

    [Fact]
    public void TestVmExecute_LoadInstruction_HexValueToMemoryLocation()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, %10") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        executor.BeginExecution();

        string? memoryValue = executor.GetValueInMemory(1);
        Assert.NotNull(memoryValue);
        Assert.Equal("16", memoryValue);
    }

    [Fact]
    public void TestVmExecute_LoadInstruction_SetMemoryLocation1ToMemoryLocation2Value()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6"), new ScriptChunkOpCode(OpCodes.Load, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        executor.BeginExecution();

        string? memoryValue1 = executor.GetValueInMemory(1);
        string? memoryValue2 = executor.GetValueInMemory(2);
        Assert.NotNull(memoryValue1);
        Assert.NotNull(memoryValue2);
        Assert.Equal("6", memoryValue1);
        Assert.Equal("6", memoryValue2);
    }
}
