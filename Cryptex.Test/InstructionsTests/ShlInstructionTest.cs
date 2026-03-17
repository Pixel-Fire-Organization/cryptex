using System.Numerics;
using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.Test.InstructionsTests;

public sealed class ShlInstructionTest
{
    [Fact]
    public void TestShl_BigShiftingFactor()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Shl, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("script", [mainChunk],
            [VmValue.FromInteger(5), VmValue.FromInteger(new BigInteger((long)int.MaxValue + 1))]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VmValue.FromInteger(5), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestShl_SmallShiftingFactor()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Shl, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("script", [mainChunk],
            [VmValue.FromInteger(5), VmValue.FromInteger(new BigInteger((long)int.MinValue - 1))]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VmValue.FromInteger(5), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestShl_CorrectShiftingFactor()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Shl, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("script", [mainChunk],
            [VmValue.FromInteger(5), VmValue.FromInteger(5)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VmValue.FromInteger(5 << 5), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestShl_FloatingShiftingFactor()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Shl, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("script", [mainChunk],
            [VmValue.FromInteger(5), VmValue.FromFloat(5.5m)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VmValue.FromInteger(5), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestShl_FloatingValue()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Shl, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("script", [mainChunk],
            [VmValue.FromFloat(5.5m), VmValue.FromInteger(5)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VmValue.FromFloat(5.5m), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestShl_CorrectValue()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Shl, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("script", [mainChunk],
            [VmValue.FromInteger(5), VmValue.FromInteger(5)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VmValue.FromInteger(5 << 5), executor.GetValueInMemory(1));
    }
}
