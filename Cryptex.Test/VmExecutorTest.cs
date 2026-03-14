using Cryptex.VM.Execution;

namespace Cryptex.Test;

public sealed class VmExecutorTest
{
    [Fact]
    public void TestBeginExecution_OnErrorInScript()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6.25"), new ScriptChunkOpCode(OpCodes.Add, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
    }
    
    [Fact]
    public void TestBeginExecution_OnNoErrorInScript()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6"), new ScriptChunkOpCode(OpCodes.Add, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());
    }
    
    [Fact]
    public void TestExecuteChunk_OnErrorInScript()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6.25"), new ScriptChunkOpCode(OpCodes.Add, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteChunk());
    }
    
    [Fact]
    public void TestExecuteChunk_OnNoErrorInScript()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6"), new ScriptChunkOpCode(OpCodes.Add, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteChunk());
    }
    
    [Fact]
    public void TestExecuteChunk_OnNonExistentScriptChunk()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6"), new ScriptChunkOpCode(OpCodes.Add, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteChunk("abc"));
    }
    
    [Fact]
    public void TestDumpMemory_FullMemory()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6"), new ScriptChunkOpCode(OpCodes.Add, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        Assert.Equal("[1]: `11`\n[2]: `6`\n", executor.DumpMemory());
    }
    
    [Fact]
    public void TestDumpMemory_EmptyMemory()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Load, "$1, #5"), new ScriptChunkOpCode(OpCodes.Load, "$2, #6"), new ScriptChunkOpCode(OpCodes.Add, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);

        Assert.Equal("<EMPTY>", executor.DumpMemory());
    }
}
