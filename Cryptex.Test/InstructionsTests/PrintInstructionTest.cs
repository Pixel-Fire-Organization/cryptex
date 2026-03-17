using Cryptex.VM.Execution;
using Cryptex.VM.Execution.OperationCodes;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Test.InstructionsTests;

public sealed class PrintInstructionTest
{
    [Fact]
    public void TestPrint_InvokesDelegate()
    {
        var printed = new List<string>();
        PrintingDelegates.WriteMessage = s => printed.Add(s);

        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load,  [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Print, [Args.Mem(1)])
        ]);
        Script script = new Script("script", [chunk], [VMValue.FromInteger(42)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Single(printed);
        Assert.Equal("42", printed[0]);
    }

    [Fact]
    public void TestPrint_FloatValue()
    {
        var printed = new List<string>();
        PrintingDelegates.WriteMessage = s => printed.Add(s);

        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load,  [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Print, [Args.Mem(1)])
        ]);
        Script script = new Script("script", [chunk], [VMValue.FromFloat(3.14m)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Single(printed);
        Assert.Equal("3.14", printed[0]);
    }

    [Fact]
    public void TestPrint_UndefinedSlot_Errors()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Print, [Args.Mem(99)])
        ]);
        Script script = new Script("script", [chunk]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());
    }

    [Fact]
    public void TestPrint_WrongArgType_Errors()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Print, [Args.Const(0)])
        ]);
        Script script = new Script("script", [chunk], [VMValue.FromInteger(1)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());
    }
}

