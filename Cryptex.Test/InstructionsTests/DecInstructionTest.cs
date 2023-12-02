using Cryptex.VM.Execution;

namespace Cryptex.Test.InstructionsTests;

public sealed class DecInstructionTest
{
    [Fact]
    public void TestVmExecute_DecrementInstruction()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Dec, "$1") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        executor.BeginExecution();

        string? memoryValue = executor.GetValueInMemory(1);
        Assert.NotNull(memoryValue);
        Assert.Equal("4", memoryValue);
    }
    
    [Fact]
    public void TestVmExecute_DecrementInstruction_Floating()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5.25"), new ScriptChunkOpCode(OpCodes.DecF, "$1") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        executor.BeginExecution();

        string? memoryValue = executor.GetValueInMemory(1);
        Assert.NotNull(memoryValue);
        Assert.Equal("4.25", memoryValue);
    }
}
