using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.Test.InstructionsTests;

public sealed class JeqInstructionTest
{
    private static ScriptChunk BuildChunk(int v1, int v2, OpCodes jumpOp, int labelIdx) =>
        new("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]), // 0
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]), // 1
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(2)]),   // 2
            new ScriptInstruction(jumpOp,        [Args.Label(labelIdx)]),       // 3
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(2)]), // 4
            new ScriptInstruction(OpCodes.Load, [Args.Mem(4), Args.Const(2)])  // 5
        ]);

    [Fact]
    public void TestJeq_Fires_WhenEqual()
    {
        var chunk = BuildChunk(5, 5, OpCodes.Jeq, 5);
        Script script = new Script("script", [chunk],
            [VmValue.FromInteger(5), VmValue.FromInteger(5), VmValue.FromInteger(1)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.True(executor.GetValueInMemory(3).IsUndefined);
        Assert.False(executor.GetValueInMemory(4).IsUndefined);
    }

    [Fact]
    public void TestJeq_DoesNotFire_WhenNotEqual()
    {
        var chunk = BuildChunk(5, 6, OpCodes.Jeq, 5);
        Script script = new Script("script", [chunk],
            [VmValue.FromInteger(5), VmValue.FromInteger(6), VmValue.FromInteger(1)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.False(executor.GetValueInMemory(3).IsUndefined);
        Assert.False(executor.GetValueInMemory(4).IsUndefined);
    }

    [Fact]
    public void TestJnq_Fires_WhenNotEqual()
    {
        var chunk = BuildChunk(5, 6, OpCodes.Jnq, 5);
        Script script = new Script("script", [chunk],
            [VmValue.FromInteger(5), VmValue.FromInteger(6), VmValue.FromInteger(1)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.True(executor.GetValueInMemory(3).IsUndefined);
        Assert.False(executor.GetValueInMemory(4).IsUndefined);
    }

    [Fact]
    public void TestJnq_DoesNotFire_WhenEqual()
    {
        var chunk = BuildChunk(5, 5, OpCodes.Jnq, 5);
        Script script = new Script("script", [chunk],
            [VmValue.FromInteger(5), VmValue.FromInteger(5), VmValue.FromInteger(1)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.False(executor.GetValueInMemory(3).IsUndefined);
    }

    [Fact]
    public void TestJls_Fires_WhenLess()
    {
        var chunk = BuildChunk(4, 5, OpCodes.Jls, 5);
        Script script = new Script("script", [chunk],
            [VmValue.FromInteger(4), VmValue.FromInteger(5), VmValue.FromInteger(1)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.True(executor.GetValueInMemory(3).IsUndefined);
    }

    [Fact]
    public void TestJgr_Fires_WhenGreater()
    {
        var chunk = BuildChunk(6, 5, OpCodes.Jgr, 5);
        Script script = new Script("script", [chunk],
            [VmValue.FromInteger(6), VmValue.FromInteger(5), VmValue.FromInteger(1)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.True(executor.GetValueInMemory(3).IsUndefined);
    }

    [Fact]
    public void TestJge_Fires_WhenGreater()
    {
        var chunk = BuildChunk(6, 5, OpCodes.Jge, 5);
        Script script = new Script("script", [chunk],
            [VmValue.FromInteger(6), VmValue.FromInteger(5), VmValue.FromInteger(1)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.True(executor.GetValueInMemory(3).IsUndefined);
    }

    [Fact]
    public void TestJge_Fires_WhenEqual()
    {
        var chunk = BuildChunk(5, 5, OpCodes.Jge, 5);
        Script script = new Script("script", [chunk],
            [VmValue.FromInteger(5), VmValue.FromInteger(5), VmValue.FromInteger(1)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.True(executor.GetValueInMemory(3).IsUndefined);
    }

    [Fact]
    public void TestJle_Fires_WhenLess()
    {
        var chunk = BuildChunk(4, 5, OpCodes.Jle, 5);
        Script script = new Script("script", [chunk],
            [VmValue.FromInteger(4), VmValue.FromInteger(5), VmValue.FromInteger(1)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.True(executor.GetValueInMemory(3).IsUndefined);
    }

    [Fact]
    public void TestJle_Fires_WhenEqual()
    {
        var chunk = BuildChunk(5, 5, OpCodes.Jle, 5);
        Script script = new Script("script", [chunk],
            [VmValue.FromInteger(5), VmValue.FromInteger(5), VmValue.FromInteger(1)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.True(executor.GetValueInMemory(3).IsUndefined);
    }

    [Fact]
    public void TestJeq_WrongArgType_Errors()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Jeq, [Args.Const(0)])
        ]);
        Script script = new Script("script", [chunk], [VmValue.FromInteger(0)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());
    }
}

