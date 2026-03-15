using Cryptex.VM.Execution;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Test.InstructionsTests;

public sealed class OrInstructionTest
{
    [Fact]
    public void TestOr_MemoryAddresses()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Load, "$2, #6"), new ScriptInstruction(OpCodes.Or, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        VMValue memoryValue1 = executor.GetValueInMemory(1);
        VMValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VMValue.FromInteger(5 | 6), memoryValue1);
        Assert.Equal(VMValue.FromInteger(6), memoryValue2);
    }
    
    [Fact]
    public void TestOr_MemoryAddressAndHexValue()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Or, "$1, %7f") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        VMValue memoryValue1 = executor.GetValueInMemory(1);
        Assert.False(memoryValue1.IsUndefined);
        Assert.Equal(VMValue.FromInteger(5 | 0x7f), memoryValue1);
    }
    
    [Fact]
    public void TestOr_MemoryAddressAndDecimalValue()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Or, "$1, #10") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.True(executor.BeginExecution());

        VMValue memoryValue1 = executor.GetValueInMemory(1);
        Assert.False(memoryValue1.IsUndefined);
        Assert.Equal(VMValue.FromInteger(5 | 10), memoryValue1);
    }
    
    [Fact]
    public void TestOr_FloatingInMemoryAddresses()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5.5"), new ScriptInstruction(OpCodes.Load, "$2, #6"), new ScriptInstruction(OpCodes.Or, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());

        VMValue memoryValue1 = executor.GetValueInMemory(1);
        VMValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VMValue.FromFloat(5.5m), memoryValue1);
        Assert.Equal(VMValue.FromInteger(6), memoryValue2);
    }
    
    [Fact]
    public void TestOr_MemoryAddresses_Floating()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Load, "$2, #6.5"), new ScriptInstruction(OpCodes.Or, "$1, $2") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());

        VMValue memoryValue1 = executor.GetValueInMemory(1);
        VMValue memoryValue2 = executor.GetValueInMemory(2);
        Assert.False(memoryValue1.IsUndefined);
        Assert.False(memoryValue2.IsUndefined);
        Assert.Equal(VMValue.FromInteger(5), memoryValue1);
        Assert.Equal(VMValue.FromFloat(6.5m), memoryValue2);
    }
    
    [Fact]
    public void TestOr_MemoryAddressAndHexValue_Floating()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Or, "$1, %5.5") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());

        VMValue memoryValue1 = executor.GetValueInMemory(1);
        Assert.False(memoryValue1.IsUndefined);
        Assert.Equal(VMValue.FromInteger(5), memoryValue1);
    }
    
    [Fact]
    public void TestOr_MemoryAddressAndDecimalValue_Floating()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Or, "$1, #10.5") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());

        VMValue memoryValue1 = executor.GetValueInMemory(1);
        Assert.False(memoryValue1.IsUndefined);
        Assert.Equal(VMValue.FromInteger(5), memoryValue1);
    }
    
    [Fact]
    public void TestOr_IncorrectFirstArgument()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptInstruction(OpCodes.Load, "$1, #5"), new ScriptInstruction(OpCodes.Or, "#1, #10") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        Assert.False(executor.BeginExecution());

        VMValue memoryValue1 = executor.GetValueInMemory(1);
        Assert.False(memoryValue1.IsUndefined);
        Assert.Equal(VMValue.FromInteger(5), memoryValue1);
    }
}
