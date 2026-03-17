using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.Test.InstructionsTests;

public sealed class ReadInstructionTest
{
    [Fact]
    public void TestRead_ValidInteger_StoresValue()
    {
        Console.SetIn(new StringReader("42"));

        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Read, [Args.Mem(1)])
        ]);
        Script script = new Script("script", [chunk]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VmValue.FromInteger(42), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestRead_InvalidInput_SetsErrorFlag()
    {
        Console.SetIn(new StringReader("notanumber"));

        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Read,     [Args.Mem(1)]),
            new ScriptInstruction(OpCodes.GetError, [Args.Mem(2)])
        ]);
        Script script = new Script("script", [chunk]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.True(executor.GetValueInMemory(1).IsUndefined);
        Assert.Equal(VmValue.FromInteger((int)ErrorCodes.Vm2014InvalidInputProvided),
            executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestRead_WrongArgType_Errors()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Read, [Args.Const(0)])
        ]);
        Script script = new Script("script", [chunk], [VmValue.FromInteger(0)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());
    }

    [Fact]
    public void TestReadLine_StoresString()
    {
        Console.SetIn(new StringReader("hello world"));

        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.ReadLine, [Args.Mem(1)])
        ]);
        Script script = new Script("script", [chunk]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        var val = executor.GetValueInMemory(1);
        Assert.True(val.IsString);
        Assert.Equal("hello world", val.AsString());
    }

    [Fact]
    public void TestReadLine_WrongArgType_Errors()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.ReadLine, [Args.Const(0)])
        ]);
        Script script = new Script("script", [chunk], [VmValue.FromInteger(0)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());
    }
}

