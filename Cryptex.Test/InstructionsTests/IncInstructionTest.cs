using Cryptex.VM.Execution;

namespace Cryptex.Test.InstructionsTests;

public sealed class IncInstructionTest
{
    [Fact]
    public void TestVmExecute_IncrementInstruction()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Inc, "$1") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        executor.BeginExecution();

        string? memoryValue = executor.GetValueInMemory(1);
        Assert.NotNull(memoryValue);
        Assert.Equal("6", memoryValue);
    }
    
    [Fact]
    public void TestVmExecute_IncrementInstruction_Floating()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5.5"), new ScriptChunkOpCode(OpCodes.IncF, "$1") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        executor.BeginExecution();

        string? memoryValue = executor.GetValueInMemory(1);
        Assert.NotNull(memoryValue);
        Assert.Equal("6.5", memoryValue);
    }
}
