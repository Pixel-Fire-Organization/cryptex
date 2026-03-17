using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.Test.InstructionsTests;

public sealed class GetErrorInstructionTest
{
    [Fact]
    public void TestGetError_WithActiveError_StoresCode()
    {
        Console.SetIn(new StringReader("bad"));

        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Read,     [Args.Mem(1)]),
            new ScriptInstruction(OpCodes.GetError, [Args.Mem(2)])
        ]);
        Script script = new Script("script", [chunk]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(
            VmValue.FromInteger((int)ErrorCodes.VM2014_InvalidInputProvided),
            executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestGetError_WithNoError_StoresZero()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load,     [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.GetError, [Args.Mem(2)])
        ]);
        Script script = new Script("script", [chunk], [VmValue.FromInteger(5)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VmValue.FromInteger(0), executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestGetError_ClearsFlag()
    {
        Console.SetIn(new StringReader("bad\nbad"));

        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Read,     [Args.Mem(1)]),
            new ScriptInstruction(OpCodes.GetError, [Args.Mem(2)]),
            new ScriptInstruction(OpCodes.Read,     [Args.Mem(3)]),
            new ScriptInstruction(OpCodes.GetError, [Args.Mem(4)])
        ]);
        Script script = new Script("script", [chunk]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(
            VmValue.FromInteger((int)ErrorCodes.VM2014_InvalidInputProvided),
            executor.GetValueInMemory(2));
        Assert.Equal(
            VmValue.FromInteger((int)ErrorCodes.VM2014_InvalidInputProvided),
            executor.GetValueInMemory(4));
    }

    [Fact]
    public void TestGetError_WrongArgType_Errors()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.GetError, [Args.Const(0)])
        ]);
        Script script = new Script("script", [chunk], [VmValue.FromInteger(0)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());
    }
}

