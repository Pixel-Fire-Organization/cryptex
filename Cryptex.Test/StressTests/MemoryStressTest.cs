using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.Test.StressTests;

/// <summary>
///     Stress tests for memory instructions (<c>load</c> and <c>free</c>).
///     Verifies that many slots can be written and released without state corruption.
/// </summary>
public sealed class MemoryStressTest
{
    [Fact]
    [Trait("Category", "Stress")]
    public void StressLoad_ManySlots_AllCorrect()
    {
        // Fill 100 independent memory slots with the same value and verify each.
        const int slotCount = 100;
        VMValue[] constants = [VMValue.FromInteger(42)];

        var instructions = new List<ScriptInstruction>();
        for (int i = 1; i <= slotCount; i++)
            instructions.Add(new ScriptInstruction(OpCodes.Load, [Args.Mem(i), Args.Const(0)]));

        Script script = Args.Build("stress_load_slots", constants, instructions.ToArray());

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        for (int i = 1; i <= slotCount; i++)
            Assert.Equal(VMValue.FromInteger(42), executor.GetValueInMemory(i));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressLoad_LargeValue_Consistent()
    {
        System.Numerics.BigInteger large = System.Numerics.BigInteger.Pow(2, 127) - 1;
        VMValue[] constants = [VMValue.FromInteger(large)];

        Script script = Args.Build("stress_load_large", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]));

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(large), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressLoad_Overwrite_ManyTimes()
    {
        // Overwrite the same slot repeatedly via a loop.
        const int iterations = 5_000;
        VMValue[] constants = [VMValue.FromInteger(0), VMValue.FromInteger(1), VMValue.FromInteger(iterations)];

        Script script = Args.Build("stress_load_overwrite", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),  // 0: $1 = 0
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(2)]),  // 1: $2 = iterations
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(0)]),  // 2: $3 = 0 (counter)
            new ScriptInstruction(OpCodes.Load, [Args.Mem(4), Args.Const(1)]),  // 3: $4 = 1 (step, reuse slot)
            new ScriptInstruction(OpCodes.Add,  [Args.Mem(1), Args.Mem(4)]),    // 4: $1 += 1
            new ScriptInstruction(OpCodes.Inc,  [Args.Mem(3)]),                 // 5: $3++
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(3), Args.Mem(2)]),    // 6: cmp $3, iterations
            new ScriptInstruction(OpCodes.Jls,  [Args.Label(4)]));              // 7: if $3 < iterations goto 4

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(iterations), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressFree_ThenReload_SlotIsUsable()
    {
        // Load a slot, free it, reload it, and verify the new value is stored correctly.
        VMValue[] constants = [VMValue.FromInteger(100), VMValue.FromInteger(200)];

        Script script = Args.Build("stress_free_reload", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Free, [Args.Mem(1)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(1)]));

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(200), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressFree_ManySlots_ThenReload_AllCorrect()
    {
        const int slotCount = 50;
        VMValue[] constants = [VMValue.FromInteger(1), VMValue.FromInteger(999)];

        var instructions = new List<ScriptInstruction>();
        for (int i = 1; i <= slotCount; i++)
            instructions.Add(new ScriptInstruction(OpCodes.Load, [Args.Mem(i), Args.Const(0)]));
        for (int i = 1; i <= slotCount; i++)
            instructions.Add(new ScriptInstruction(OpCodes.Free, [Args.Mem(i)]));
        for (int i = 1; i <= slotCount; i++)
            instructions.Add(new ScriptInstruction(OpCodes.Load, [Args.Mem(i), Args.Const(1)]));

        Script script = Args.Build("stress_free_many", constants, instructions.ToArray());

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        for (int i = 1; i <= slotCount; i++)
            Assert.Equal(VMValue.FromInteger(999), executor.GetValueInMemory(i));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressLoad_NegativeLargeValue_Consistent()
    {
        System.Numerics.BigInteger large = -System.Numerics.BigInteger.Pow(2, 127);
        VMValue[] constants = [VMValue.FromInteger(large)];

        Script script = Args.Build("stress_load_neg_large", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]));

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(large), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressLoad_FloatSlots_AllCorrect()
    {
        const int slotCount = 50;
        VMValue[] constants = [VMValue.FromFloat(3.14159m)];

        var instructions = new List<ScriptInstruction>();
        for (int i = 1; i <= slotCount; i++)
            instructions.Add(new ScriptInstruction(OpCodes.Load, [Args.Mem(i), Args.Const(0)]));

        Script script = Args.Build("stress_load_float_slots", constants, instructions.ToArray());

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        for (int i = 1; i <= slotCount; i++)
            Assert.Equal(VMValue.FromFloat(3.14159m), executor.GetValueInMemory(i));
    }
}
