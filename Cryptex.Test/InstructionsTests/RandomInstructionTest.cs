using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.Test.InstructionsTests;

public sealed class RandomInstructionTest
{
    [Fact]
    public void TestRandom_StoresInteger()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Random, [Args.Mem(1)])
        ]);
        Script script = new Script("script", [chunk]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.True(executor.GetValueInMemory(1).IsInteger);
    }

    [Fact]
    public void TestRandomF_StoresFloat()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.RandomF, [Args.Mem(1)])
        ]);
        Script script = new Script("script", [chunk]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        var val = executor.GetValueInMemory(1);
        Assert.True(val.IsFloat);
        Assert.True(val.AsFloat() is >= 0m and <= 1m);
    }

    [Fact]
    public void TestRandom_WrongArgType_Errors()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Random, [Args.Const(0)])
        ]);
        Script script = new Script("script", [chunk], [VmValue.FromInteger(0)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());
    }

    [Fact]
    public void TestRandomF_WrongArgType_Errors()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.RandomF, [Args.Const(0)])
        ]);
        Script script = new Script("script", [chunk], [VmValue.FromInteger(0)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());
    }
}

