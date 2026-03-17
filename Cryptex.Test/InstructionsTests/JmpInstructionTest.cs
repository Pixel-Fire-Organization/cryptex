using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.Test.InstructionsTests;

public sealed class JmpInstructionTest
{
    [Fact]
    public void TestJmp_SkipsInstruction()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]), // 0
            new ScriptInstruction(OpCodes.Jmp,  [Args.Label(3)]),              // 1 → jump to 3
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(0)]), // 2 SKIPPED
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(0)])  // 3
        ]);
        Script script = new Script("script", [chunk], [VmValue.FromInteger(5)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.False(executor.GetValueInMemory(1).IsUndefined);
        Assert.True(executor.GetValueInMemory(2).IsUndefined);
        Assert.False(executor.GetValueInMemory(3).IsUndefined);
    }

    [Fact]
    public void TestJmp_DoesNotClearCompareFlag()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]), // 0
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(0)]), // 1
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(2)]),   // 2 → Equals
            new ScriptInstruction(OpCodes.Jmp,  [Args.Label(4)]),              // 3 → unconditional, flag preserved
            new ScriptInstruction(OpCodes.Jeq,  [Args.Label(6)]),              // 4 → fires (Equals still set)
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(1)]), // 5 SKIPPED
            new ScriptInstruction(OpCodes.Load, [Args.Mem(4), Args.Const(1)])  // 6
        ]);
        Script script = new Script("script", [chunk],
            [VmValue.FromInteger(5), VmValue.FromInteger(99)]);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.True(executor.GetValueInMemory(3).IsUndefined);
        Assert.False(executor.GetValueInMemory(4).IsUndefined);
    }

    [Fact]
    public void TestJmp_InvalidLabel_Errors()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Jmp, [Args.Label(999)])
        ]);
        Script script = new Script("script", [chunk]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());
    }

    [Fact]
    public void TestJmp_WrongArgType_Errors()
    {
        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Jmp, [Args.Const(0)])
        ]);
        Script script = new Script("script", [chunk], [VmValue.FromInteger(0)]);

        Executor executor = new Executor(script);
        Assert.False(executor.ExecuteScript());
    }
}

