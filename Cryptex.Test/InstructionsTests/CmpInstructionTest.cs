using Cryptex.VM.Execution;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Test.InstructionsTests;

public sealed class CmpInstructionTest
{
    [Fact]
    public void TestCmp_IntegersEqual_SetsEqualsThenJeqFires()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]), // 0
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(0)]), // 1
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(2)]),   // 2
            new ScriptInstruction(OpCodes.Jeq,  [Args.Label(5)]),              // 3 → fires → jump to 5
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(1)]), // 4 SKIPPED
            new ScriptInstruction(OpCodes.Load, [Args.Mem(4), Args.Const(1)])  // 5
        ]);
        Script script = new Script("script", [chunk],
            [VMValue.FromInteger(5), VMValue.FromInteger(99)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.True(executor.GetValueInMemory(3).IsUndefined);
        Assert.False(executor.GetValueInMemory(4).IsUndefined);
    }

    [Fact]
    public void TestCmp_IntegersGreater_SetsGreater()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]), // 0: $1 = 6
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]), // 1: $2 = 5
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(2)]),   // 2: Greater
            new ScriptInstruction(OpCodes.Jgr,  [Args.Label(5)]),              // 3 → fires
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(1)]), // 4 SKIPPED
            new ScriptInstruction(OpCodes.Load, [Args.Mem(4), Args.Const(1)])  // 5
        ]);
        Script script = new Script("script", [chunk],
            [VMValue.FromInteger(6), VMValue.FromInteger(5), VMValue.FromInteger(99)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.True(executor.GetValueInMemory(3).IsUndefined);
        Assert.False(executor.GetValueInMemory(4).IsUndefined);
    }

    [Fact]
    public void TestCmp_IntegersLess_SetsLess()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]), // 0: $1 = 4
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]), // 1: $2 = 5
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(2)]),   // 2: Less
            new ScriptInstruction(OpCodes.Jls,  [Args.Label(5)]),              // 3 → fires
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(1)]), // 4 SKIPPED
            new ScriptInstruction(OpCodes.Load, [Args.Mem(4), Args.Const(1)])  // 5
        ]);
        Script script = new Script("script", [chunk],
            [VMValue.FromInteger(4), VMValue.FromInteger(5), VMValue.FromInteger(99)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.True(executor.GetValueInMemory(3).IsUndefined);
        Assert.False(executor.GetValueInMemory(4).IsUndefined);
    }

    [Fact]
    public void TestCmp_FloatsEqual()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(2)]),
            new ScriptInstruction(OpCodes.Jeq,  [Args.Label(5)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(4), Args.Const(1)])
        ]);
        Script script = new Script("script", [chunk],
            [VMValue.FromFloat(3.14m), VMValue.FromInteger(99)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.True(executor.GetValueInMemory(3).IsUndefined);
    }

    [Fact]
    public void TestCmp_TypeMismatch_Errors()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(2)])
        ]);
        Script script = new Script("script", [chunk],
            [VMValue.FromInteger(5), VMValue.FromFloat(5.0m)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());
    }

    [Fact]
    public void TestCmp_WrongArgType_Errors()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Const(0)])
        ]);
        Script script = new Script("script", [chunk], [VMValue.FromInteger(5)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());
    }
}

