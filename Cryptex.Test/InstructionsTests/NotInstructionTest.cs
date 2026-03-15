using Cryptex.VM.Execution;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Test.InstructionsTests;

public sealed class NotInstructionTest
{
    [Fact]
    public void TestNot_ValidAddress()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Not, "$1") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        VMValue memoryValue1 = executor.GetValueInMemory(1);
        Assert.False(memoryValue1.IsUndefined);
        Assert.Equal(VMValue.FromInteger(~5), memoryValue1);
    }
    
    [Fact]
    public void TestNot_Floating()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5.5"), new ScriptInstruction(OpCodes.Not, "$1") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());

        VMValue memoryValue1 = executor.GetValueInMemory(1);
        Assert.False(memoryValue1.IsUndefined);
        Assert.Equal(VMValue.FromFloat(5.5m), memoryValue1);
    }
}
