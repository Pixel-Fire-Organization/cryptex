using Cryptex.VM.Execution;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Test.InstructionsTests;

public sealed class CrashInstructionTest
{
    [Fact]
    public void TestCrash_MemoryAddress()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Crash, "$1") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
    }
    
    [Fact]
    public void TestCrash_HexValue()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Crash, "%7f") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
    }
    
    [Fact]
    public void TestCrash_IntegerAddress()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Crash, "#2000") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
    }
    
    [Fact]
    public void TestCrash_InvalidErrorCode()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Crash, "#80000") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
    }
}
