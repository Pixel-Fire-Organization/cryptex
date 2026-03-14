using Cryptex.VM.Execution;

namespace Cryptex.Test.InstructionsTests;

public sealed class ExitInstructionTest
{
    [Fact]
    public void TestExit_IntegerExitCode()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Exit, "#0") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());
        Assert.Equal(0, executor.GetExitCode());
    }
    
    [Fact]
    public void TestExit_HexExitCode()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Exit, "%7f") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        Assert.Equal(-1, executor.GetExitCode());
    }
    
    [Fact]
    public void TestExit_ExitCodeInMemory()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Exit, "$25") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        Assert.Equal(-1, executor.GetExitCode());
    }
}
