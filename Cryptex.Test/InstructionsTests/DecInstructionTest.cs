using Cryptex.VM.Execution;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Test.InstructionsTests;

public sealed class DecInstructionTest
{
    [Fact]
    public void TestDec_CorrectValue()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Dec, "$1") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        VMValue memoryValue = executor.GetValueInMemory(1);
        Assert.False(memoryValue.IsUndefined);
        Assert.Equal(VMValue.FromInteger(4), memoryValue);
    }
    
    [Fact]
    public void TestDec_Floating()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5.25"), new ScriptInstruction(OpCodes.DecF, "$1") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        VMValue memoryValue = executor.GetValueInMemory(1);
        Assert.False(memoryValue.IsUndefined);
        Assert.Equal(VMValue.FromFloat(4.25m), memoryValue);
    }
}
