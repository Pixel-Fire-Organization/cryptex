using System.Numerics;
using Cryptex.VM.Execution;
using Cryptex.VM.Execution.OperationCodes;
using Cryptex.VM.Execution.Scripts;

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
            [VMValue.FromInteger(5), VMValue.FromInteger(new BigInteger((long)int.MaxValue + 1))]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(5), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestShl_SmallShiftingFactor()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Shl, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("script", [mainChunk],
            [VMValue.FromInteger(5), VMValue.FromInteger(new BigInteger((long)int.MinValue - 1))]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(5), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestShl_CorrectShiftingFactor()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Shl, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("script", [mainChunk],
            [VMValue.FromInteger(5), VMValue.FromInteger(5)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(5 << 5), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestShl_FloatingShiftingFactor()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Shl, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("script", [mainChunk],
            [VMValue.FromInteger(5), VMValue.FromFloat(5.5m)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(5), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestShl_FloatingValue()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Shl, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("script", [mainChunk],
            [VMValue.FromFloat(5.5m), VMValue.FromInteger(5)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromFloat(5.5m), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestShl_CorrectValue()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Shl, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("script", [mainChunk],
            [VMValue.FromInteger(5), VMValue.FromInteger(5)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(5 << 5), executor.GetValueInMemory(1));
    }
}
