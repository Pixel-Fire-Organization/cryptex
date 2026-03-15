using Cryptex.VM.Execution;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Test.InstructionsTests;

public sealed class SubInstructionTest
{
    [Fact]
    public void TestSub_CorrectValues()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Load, "$2, #6"), new ScriptInstruction(OpCodes.Sub, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        VMValue memoryValue1 = executor.GetValueInMemory(1);
        VMValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VMValue.FromInteger(-1), memoryValue1);
        Assert.Equal(VMValue.FromInteger(6), memoryValue2);
    }
    
    [Fact]
    public void TestSubf_CorrectValues()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5.25"), new ScriptInstruction(OpCodes.Load, "$2, #6.25"), new ScriptInstruction(OpCodes.SubF, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        VMValue memoryValue1 = executor.GetValueInMemory(1);
        VMValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VMValue.FromFloat(-1.00m), memoryValue1);
        Assert.Equal(VMValue.FromFloat(6.25m), memoryValue2);
    }
    
    [Fact]
    public void TestSub_FloatingAndInteger()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5.25"), new ScriptInstruction(OpCodes.Load, "$2, #6"), new ScriptInstruction(OpCodes.Sub, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        
        VMValue memoryValue1 = executor.GetValueInMemory(1);
        VMValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VMValue.FromFloat(5.25m), memoryValue1);
        Assert.Equal(VMValue.FromInteger(6), memoryValue2);
    }
    
    [Fact]
    public void TestSubf_FloatingInteger()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5.25"), new ScriptInstruction(OpCodes.Load, "$2, #6"), new ScriptInstruction(OpCodes.SubF, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        
        VMValue memoryValue1 = executor.GetValueInMemory(1);
        VMValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VMValue.FromFloat(5.25m), memoryValue1);
        Assert.Equal(VMValue.FromInteger(6), memoryValue2);
    }
    
    [Fact]
    public void TestSub_ArgumentNotMemory()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Load, "$2, #6"), new ScriptInstruction(OpCodes.Sub, "$1, #2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        
        VMValue memoryValue1 = executor.GetValueInMemory(1);
        VMValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VMValue.FromInteger(5), memoryValue1);
        Assert.Equal(VMValue.FromInteger(6), memoryValue2);
    }
    
    [Fact]
    public void TestSubf_ArgumentNotMemory()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5.25"), new ScriptInstruction(OpCodes.Load, "$2, #6.25"), new ScriptInstruction(OpCodes.SubF, "$1, #2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());

        VMValue memoryValue1 = executor.GetValueInMemory(1);
        VMValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VMValue.FromFloat(5.25m), memoryValue1);
        Assert.Equal(VMValue.FromFloat(6.25m), memoryValue2);
    }
    
    [Fact]
    public void TestSub_TooFewArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Load, "$2, #6"), new ScriptInstruction(OpCodes.Sub, "$1") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        
        VMValue memoryValue1 = executor.GetValueInMemory(1);
        VMValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VMValue.FromInteger(5), memoryValue1);
        Assert.Equal(VMValue.FromInteger(6), memoryValue2);
    }
    
    [Fact]
    public void TestSubf_TooFewArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5.25"), new ScriptInstruction(OpCodes.Load, "$2, #6.25"), new ScriptInstruction(OpCodes.SubF, "$1") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        
        VMValue memoryValue1 = executor.GetValueInMemory(1);
        VMValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VMValue.FromFloat(5.25m), memoryValue1);
        Assert.Equal(VMValue.FromFloat(6.25m), memoryValue2);
    }
    
    [Fact]
    public void TestSub_TooMuchArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Load, "$2, #6"), new ScriptInstruction(OpCodes.Sub, "$1, $2, $3") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        
        VMValue memoryValue1 = executor.GetValueInMemory(1);
        VMValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VMValue.FromInteger(5), memoryValue1);
        Assert.Equal(VMValue.FromInteger(6), memoryValue2);
    }
    
    [Fact]
    public void TestSubf_TooMuchArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5.25"), new ScriptInstruction(OpCodes.Load, "$2, #6.25"), new ScriptInstruction(OpCodes.SubF, "$1, $2, $3") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        
        VMValue memoryValue1 = executor.GetValueInMemory(1);
        VMValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VMValue.FromFloat(5.25m), memoryValue1);
        Assert.Equal(VMValue.FromFloat(6.25m), memoryValue2);
    }
    
    [Fact]
    public void TestSub_NoArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Load, "$2, #6"), new ScriptInstruction(OpCodes.Sub, "") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        
        VMValue memoryValue1 = executor.GetValueInMemory(1);
        VMValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VMValue.FromInteger(5), memoryValue1);
        Assert.Equal(VMValue.FromInteger(6), memoryValue2);
    }
    
    [Fact]
    public void TestSubf_NoArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5.25"), new ScriptInstruction(OpCodes.Load, "$2, #6.25"), new ScriptInstruction(OpCodes.SubF, "") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());
        
        VMValue memoryValue1 = executor.GetValueInMemory(1);
        VMValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VMValue.FromFloat(5.25m), memoryValue1);
        Assert.Equal(VMValue.FromFloat(6.25m), memoryValue2);
    }
}
