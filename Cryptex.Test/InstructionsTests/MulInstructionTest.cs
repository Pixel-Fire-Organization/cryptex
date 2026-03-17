using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.Test.InstructionsTests;

public sealed class MulInstructionTest
{
    // Shared constants: [0]=5, [1]=6, [2]=5.25, [3]=6.25
    private static readonly VmValue[] Constants =
    [
        VmValue.FromInteger(5), VmValue.FromInteger(6),
        VmValue.FromFloat(5.25m), VmValue.FromFloat(6.25m)
    ];

    [Fact]
    public void TestMul_CorrectValues()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Mul, [Args.Mem(1), Args.Mem(2)])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VmValue.FromInteger(30), executor.GetValueInMemory(1));
        Assert.Equal(VmValue.FromInteger(6),  executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestMulf_CorrectValues()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(2)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(3)]),
            new ScriptInstruction(OpCodes.MulF, [Args.Mem(1), Args.Mem(2)])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VmValue.FromFloat(32.8125m), executor.GetValueInMemory(1));
        Assert.Equal(VmValue.FromFloat(6.25m),    executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestMul_FloatingAndInteger()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(2)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Mul, [Args.Mem(1), Args.Mem(2)])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VmValue.FromFloat(5.25m), executor.GetValueInMemory(1));
        Assert.Equal(VmValue.FromInteger(6),   executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestMulf_FloatingInteger()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(2)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.MulF, [Args.Mem(1), Args.Mem(2)])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VmValue.FromFloat(5.25m), executor.GetValueInMemory(1));
        Assert.Equal(VmValue.FromInteger(6),   executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestMul_ArgumentNotMemory()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Mul, [Args.Mem(1), Args.Const(0)])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VmValue.FromInteger(5), executor.GetValueInMemory(1));
        Assert.Equal(VmValue.FromInteger(6), executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestMulf_ArgumentNotMemory()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(2)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(3)]),
            new ScriptInstruction(OpCodes.MulF, [Args.Mem(1), Args.Const(0)])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VmValue.FromFloat(5.25m), executor.GetValueInMemory(1));
        Assert.Equal(VmValue.FromFloat(6.25m), executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestMul_TooFewArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Mul, [Args.Mem(1)])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VmValue.FromInteger(5), executor.GetValueInMemory(1));
        Assert.Equal(VmValue.FromInteger(6), executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestMulf_TooFewArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(2)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(3)]),
            new ScriptInstruction(OpCodes.MulF, [Args.Mem(1)])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VmValue.FromFloat(5.25m), executor.GetValueInMemory(1));
        Assert.Equal(VmValue.FromFloat(6.25m), executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestMul_TooMuchArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Mul, [Args.Mem(1), Args.Mem(2), Args.Mem(3)])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VmValue.FromInteger(5), executor.GetValueInMemory(1));
        Assert.Equal(VmValue.FromInteger(6), executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestMulf_TooMuchArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(2)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(3)]),
            new ScriptInstruction(OpCodes.MulF, [Args.Mem(1), Args.Mem(2), Args.Mem(3)])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VmValue.FromFloat(5.25m), executor.GetValueInMemory(1));
        Assert.Equal(VmValue.FromFloat(6.25m), executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestMul_NoArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Mul, [ScriptInstructionArgument.Default])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VmValue.FromInteger(5), executor.GetValueInMemory(1));
        Assert.Equal(VmValue.FromInteger(6), executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestMulf_NoArguments()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(2)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(3)]),
            new ScriptInstruction(OpCodes.MulF, [ScriptInstructionArgument.Default])
        ]);
        Script script = new Script("script", [mainChunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VmValue.FromFloat(5.25m), executor.GetValueInMemory(1));
        Assert.Equal(VmValue.FromFloat(6.25m), executor.GetValueInMemory(2));
    }
}
