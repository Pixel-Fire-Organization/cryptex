using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.Test.InstructionsTests;

public sealed class AddImmInstructionTest
{
    private static readonly VMValue[] Constants =
    [
        VMValue.FromInteger(10), VMValue.FromInteger(3),
        VMValue.FromFloat(2.5m)
    ];

    [Fact]
    public void TestAddImm_CorrectValues()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load,   [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.AddImm, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("script", [chunk], Constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(13), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestSubImm_CorrectValues()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load,   [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.SubImm, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("script", [chunk], Constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(7), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestMulImm_CorrectValues()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load,   [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.MulImm, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("script", [chunk], Constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(30), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestDivImm_CorrectValues()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load,   [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.DivImm, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("script", [chunk], Constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(3), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestModImm_CorrectValues()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load,   [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.ModImm, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("script", [chunk], Constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(1), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestAddImm_FloatInMemory()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load,   [Args.Mem(1), Args.Const(2)]),
            new ScriptInstruction(OpCodes.AddImm, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("script", [chunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromFloat(2.5m), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestAddImm_FloatConstant()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load,   [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.AddImm, [Args.Mem(1), Args.Const(2)])
        ]);
        Script script = new Script("script", [chunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(10), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestAddImm_MemArgInsteadOfConst()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load,   [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.AddImm, [Args.Mem(1), Args.Mem(2)])
        ]);
        Script script = new Script("script", [chunk], Constants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());
    }

    [Fact]
    public void TestDivImm_ByZero()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load,   [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.DivImm, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("script", [chunk],
            [VMValue.FromInteger(10), VMValue.FromInteger(0)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());
    }
}

