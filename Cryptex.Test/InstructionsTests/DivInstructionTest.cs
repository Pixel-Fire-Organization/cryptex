using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.Test.InstructionsTests;

public sealed class DivInstructionTest
{
    // Shared constants: [0]=5, [1]=6, [2]=5.25, [3]=6.25
    private static readonly VMValue[] Constants =
    [
        VMValue.FromInteger(5), VMValue.FromInteger(6),
        VMValue.FromFloat(5.25m), VMValue.FromFloat(6.25m)
    ];

    [Fact]
    public void TestDiv_CorrectValues()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Div, [Args.Mem(1), Args.Mem(2)])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(0), executor.GetValueInMemory(1));
        Assert.Equal(VMValue.FromInteger(6), executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestDivf_CorrectValues()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(2)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(3)]),
            new ScriptInstruction(OpCodes.DivF, [Args.Mem(1), Args.Mem(2)])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromFloat(0.84m), executor.GetValueInMemory(1));
        Assert.Equal(VMValue.FromFloat(6.25m), executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestDiv_FloatingAndInteger()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(2)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Div, [Args.Mem(1), Args.Mem(2)])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromFloat(5.25m), executor.GetValueInMemory(1));
        Assert.Equal(VMValue.FromInteger(6),   executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestDivf_FloatingInteger()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(2)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.DivF, [Args.Mem(1), Args.Mem(2)])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromFloat(5.25m), executor.GetValueInMemory(1));
        Assert.Equal(VMValue.FromInteger(6),   executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestDiv_ArgumentNotMemory()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Div, [Args.Mem(1), Args.Const(0)])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(5), executor.GetValueInMemory(1));
        Assert.Equal(VMValue.FromInteger(6), executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestDivf_ArgumentNotMemory()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(2)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(3)]),
            new ScriptInstruction(OpCodes.DivF, [Args.Mem(1), Args.Const(0)])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromFloat(5.25m), executor.GetValueInMemory(1));
        Assert.Equal(VMValue.FromFloat(6.25m), executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestDiv_TooFewArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Div, [Args.Mem(1)])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(5), executor.GetValueInMemory(1));
        Assert.Equal(VMValue.FromInteger(6), executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestDivf_TooFewArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(2)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(3)]),
            new ScriptInstruction(OpCodes.DivF, [Args.Mem(1)])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromFloat(5.25m), executor.GetValueInMemory(1));
        Assert.Equal(VMValue.FromFloat(6.25m), executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestDiv_TooMuchArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Div, [Args.Mem(1), Args.Mem(2), Args.Mem(3)])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(5), executor.GetValueInMemory(1));
        Assert.Equal(VMValue.FromInteger(6), executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestDivf_TooMuchArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(2)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(3)]),
            new ScriptInstruction(OpCodes.DivF, [Args.Mem(1), Args.Mem(2), Args.Mem(3)])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromFloat(5.25m), executor.GetValueInMemory(1));
        Assert.Equal(VMValue.FromFloat(6.25m), executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestDiv_NoArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Div, [ScriptInstructionArgument.Default])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(5), executor.GetValueInMemory(1));
        Assert.Equal(VMValue.FromInteger(6), executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestDivf_NoArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(2)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(3)]),
            new ScriptInstruction(OpCodes.DivF, [ScriptInstructionArgument.Default])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromFloat(5.25m), executor.GetValueInMemory(1));
        Assert.Equal(VMValue.FromFloat(6.25m), executor.GetValueInMemory(2));
    }
}
