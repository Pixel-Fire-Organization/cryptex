using Cryptex.VM.Execution;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Test.InstructionsTests;

public sealed class DivInstructionTest
{
    [Fact]
    public void TestDiv_CorrectValues()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Load, "$2, #6"), new ScriptInstruction(OpCodes.Div, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        VMValue memoryValue1 = executor.GetValueInMemory(1);
        VMValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VMValue.FromInteger(0), memoryValue1);
        Assert.Equal(VMValue.FromInteger(6), memoryValue2);
    }
    
    [Fact]
    public void TestDivf_CorrectValues()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5.25"), new ScriptInstruction(OpCodes.Load, "$2, #6.25"), new ScriptInstruction(OpCodes.DivF, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        VMValue memoryValue1 = executor.GetValueInMemory(1);
        VMValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VMValue.FromFloat(0.84m), memoryValue1);
        Assert.Equal(VMValue.FromFloat(6.25m), memoryValue2);
    }
    
    [Fact]
    public void TestDiv_FloatingAndInteger()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5.25"), new ScriptInstruction(OpCodes.Load, "$2, #6"), new ScriptInstruction(OpCodes.Div, "$1, $2") });
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
    public void TestDivf_FloatingInteger()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5.25"), new ScriptInstruction(OpCodes.Load, "$2, #6"), new ScriptInstruction(OpCodes.DivF, "$1, $2") });
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
    public void TestDiv_ArgumentNotMemory()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Load, "$2, #6"), new ScriptInstruction(OpCodes.Div, "$1, #2") });
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
    public void TestDivf_ArgumentNotMemory()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5.25"), new ScriptInstruction(OpCodes.Load, "$2, #6.25"), new ScriptInstruction(OpCodes.DivF, "$1, #2") });
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
    public void TestDiv_TooFewArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Load, "$2, #6"), new ScriptInstruction(OpCodes.Div, "$1") });
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
    public void TestDivf_TooFewArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5.25"), new ScriptInstruction(OpCodes.Load, "$2, #6.25"), new ScriptInstruction(OpCodes.DivF, "$1") });
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
    public void TestDiv_TooMuchArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Load, "$2, #6"), new ScriptInstruction(OpCodes.Div, "$1, $2, $3") });
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
    public void TestDivf_TooMuchArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5.25"), new ScriptInstruction(OpCodes.Load, "$2, #6.25"), new ScriptInstruction(OpCodes.DivF, "$1, $2, $3") });
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
    public void TestDiv_NoArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Load, "$2, #6"), new ScriptInstruction(OpCodes.Div, "") });
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
    public void TestDivf_NoArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5.25"), new ScriptInstruction(OpCodes.Load, "$2, #6.25"), new ScriptInstruction(OpCodes.DivF, "") });
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
