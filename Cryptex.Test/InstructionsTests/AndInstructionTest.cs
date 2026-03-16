using Cryptex.VM.Execution;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Test.InstructionsTests;

public sealed class AndInstructionTest
{
    private static readonly VMValue[] IntConstants = [VMValue.FromInteger(5), VMValue.FromInteger(6)];
    private static readonly VMValue[] FloatConstants = [VMValue.FromFloat(5.5m), VMValue.FromInteger(6)];

    [Fact]
    public void TestAnd_MemoryAddresses()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.And, [Args.Mem(1), Args.Mem(2)])
        ]);
        Script script = new Script("script", [mainChunk], IntConstants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(5 & 6), executor.GetValueInMemory(1));
        Assert.Equal(VMValue.FromInteger(6),     executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestAnd_MemoryAddressAndHexValue()
    {
        // HexConstant type — And only accepts MemoryAddress for the second operand.
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.And, [Args.Mem(1), Args.HexConst(1)])
        ]);
        Script script = new Script("script", [mainChunk], IntConstants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(5), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestAnd_MemoryAddressAndDecimalValue()
    {
        // Constant type — And only accepts MemoryAddress for the second operand.
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.And, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("script", [mainChunk], IntConstants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(5), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestAnd_FloatingInMemoryAddresses()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.And, [Args.Mem(1), Args.Mem(2)])
        ]);
        Script script = new Script("script", [mainChunk], FloatConstants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromFloat(5.5m),  executor.GetValueInMemory(1));
        Assert.Equal(VMValue.FromInteger(6),   executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestAnd_MemoryAddresses_Floating()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.And, [Args.Mem(1), Args.Mem(2)])
        ]);
        Script script = new Script("script", [mainChunk],
            [VMValue.FromInteger(5), VMValue.FromFloat(6.5m)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(5),   executor.GetValueInMemory(1));
        Assert.Equal(VMValue.FromFloat(6.5m),  executor.GetValueInMemory(2));
    }

    [Fact]
    public void TestAnd_MemoryAddressAndHexValue_Floating()
    {
        // HexConstant type — invalid type, fails immediately.
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.And, [Args.Mem(1), Args.HexConst(1)])
        ]);
        Script script = new Script("script", [mainChunk], IntConstants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(5), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestAnd_MemoryAddressAndDecimalValue_Floating()
    {
        // Constant type — invalid type, fails immediately.
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.And, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("script", [mainChunk],
            [VMValue.FromInteger(5), VMValue.FromFloat(10.5m)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(5), executor.GetValueInMemory(1));
    }

    [Fact]
    public void TestAnd_IncorrectFirstArgument()
    {
        // First arg is Constant — And requires MemoryAddress for both operands.
        ScriptChunk mainChunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.And, [Args.Const(0), Args.Const(1)])
        ]);
        Script script = new Script("script", [mainChunk], IntConstants);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(5), executor.GetValueInMemory(1));
    }
}
