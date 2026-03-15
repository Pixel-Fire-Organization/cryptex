using Cryptex.VM.Execution;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Test.InstructionsTests;

public sealed class NopInstructionTest
{
    [Fact]
    public void TestNop_FloatingTime()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Nop, "#5.5") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
    }
    
    [Fact]
    public void TestNop_HexValue()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Nop, "%5") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
    }
    
    [Fact]
    public void TestNop_MemoryLocation()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Nop, "$5") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
    }
    
    [Fact]
    public void TestNop_CorrectTime()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Nop, "#10") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());
    }
}
