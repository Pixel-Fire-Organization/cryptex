using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.Test.StressTests;

/// <summary>
///     Stress tests for bitwise instructions.
///     Verifies consistent results under repeated execution and with extreme bit patterns.
/// </summary>
public sealed class BitwiseStressTest
{
    [Fact]
    [Trait("Category", "Stress")]
    public void StressAnd_AllBitsSet_CorrectResult()
    {
        // 0xFF & 0xFF = 0xFF — both operands all 1s must remain all 1s.
        VMValue[] constants = [VMValue.FromInteger(0xFF), VMValue.FromInteger(0xFF)];

        Script script = Args.Build("stress_and_allbits", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.And,  [Args.Mem(1), Args.Mem(2)]));

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(0xFF), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressAnd_ManyIterations_ConsistentResult()
    {
        const int iterations = 10_000;
        // $1 = 0xFFFF, $2 = 0xF0F0 — repeated AND must converge immediately and stay constant.
        VMValue[] constants = [VMValue.FromInteger(0xFFFF), VMValue.FromInteger(0xF0F0),
                                VMValue.FromInteger(iterations)];

        Script script = Args.Build("stress_and_many", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),  // 0: $1 = 0xFFFF
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),  // 1: $2 = 0xF0F0
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(2)]),  // 2: $3 = counter limit
            new ScriptInstruction(OpCodes.Load, [Args.Mem(4), Args.Const(0)]),  // 3: $4 = 0 (counter init, reuse const)
            new ScriptInstruction(OpCodes.And,  [Args.Mem(1), Args.Mem(2)]),    // 4: $1 &= $2
            new ScriptInstruction(OpCodes.Inc,  [Args.Mem(4)]),                 // 5: $4++
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(4), Args.Mem(3)]),    // 6: cmp counter, limit
            new ScriptInstruction(OpCodes.Jls,  [Args.Label(4)]));              // 7: if counter < limit goto 4

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        // After repeated AND with the same mask, result is idempotent.
        Assert.Equal(VMValue.FromInteger(0xF0F0), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressOr_AllZeros_CorrectResult()
    {
        // 0 | 0xFF = 0xFF
        VMValue[] constants = [VMValue.FromInteger(0), VMValue.FromInteger(0xFF)];

        Script script = Args.Build("stress_or_zeros", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Or,   [Args.Mem(1), Args.Mem(2)]));

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(0xFF), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressOr_ManyIterations_Idempotent()
    {
        const int iterations = 10_000;
        VMValue[] constants = [VMValue.FromInteger(0), VMValue.FromInteger(0xABCD),
                                VMValue.FromInteger(iterations)];

        Script script = Args.Build("stress_or_many", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),  // 0: $1 = 0
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),  // 1: $2 = 0xABCD
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(2)]),  // 2: $3 = limit
            new ScriptInstruction(OpCodes.Load, [Args.Mem(4), Args.Const(0)]),  // 3: $4 = 0 (counter)
            new ScriptInstruction(OpCodes.Or,   [Args.Mem(1), Args.Mem(2)]),    // 4: $1 |= $2
            new ScriptInstruction(OpCodes.Inc,  [Args.Mem(4)]),                 // 5: $4++
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(4), Args.Mem(3)]),    // 6: cmp $4, limit
            new ScriptInstruction(OpCodes.Jls,  [Args.Label(4)]));              // 7: if $4 < limit goto 4

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(0xABCD), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressXor_ToggleManyTimes_ConsistentResult()
    {
        // XOR with the same value an even number of times returns the original.
        const int iterations = 10_000; // even
        VMValue[] constants = [VMValue.FromInteger(0x1234), VMValue.FromInteger(0xFFFF),
                                VMValue.FromInteger(iterations)];

        Script script = Args.Build("stress_xor_toggle", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),  // 0: $1 = 0x1234
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),  // 1: $2 = 0xFFFF
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(2)]),  // 2: $3 = limit
            new ScriptInstruction(OpCodes.Load, [Args.Mem(4), Args.Const(0)]),  // 3: $4 = 0 (counter init via const[0] reuse)
            new ScriptInstruction(OpCodes.Xor,  [Args.Mem(1), Args.Mem(2)]),    // 4: $1 ^= $2
            new ScriptInstruction(OpCodes.Inc,  [Args.Mem(4)]),                 // 5: $4++
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(4), Args.Mem(3)]),    // 6: cmp $4, limit
            new ScriptInstruction(OpCodes.Jls,  [Args.Label(4)]));              // 7: if $4 < limit goto 4

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        // Even number of XOR with same value → original.
        Assert.Equal(VMValue.FromInteger(0x1234), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressNot_DoubleNegation_ReturnsOriginal()
    {
        VMValue[] constants = [VMValue.FromInteger(0x12345678)];

        Script script = Args.Build("stress_not_double", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Not,  [Args.Mem(1)]),
            new ScriptInstruction(OpCodes.Not,  [Args.Mem(1)]));

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(0x12345678), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressShl_ByOne_ManyTimes()
    {
        // Shift left by 1 many times; each shift doubles the value.
        // SHL requires the shift amount as a Constant arg (not MemoryAddress).
        // Uses a countdown counter ($3) to perform exactly `shifts` iterations.
        const int shifts = 8;
        VMValue[] constants = [VMValue.FromInteger(1),      // const[0] = 1 (initial value AND shift amount)
                                VMValue.FromInteger(shifts), // const[1] = 8 (countdown)
                                VMValue.FromInteger(0)];     // const[2] = 0 (compare target)

        Script script = Args.Build("stress_shl_many", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),  // 0: $1 = 1
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(1)]),  // 1: $3 = shifts (countdown)
            new ScriptInstruction(OpCodes.Load, [Args.Mem(4), Args.Const(2)]),  // 2: $4 = 0 (compare target)
            new ScriptInstruction(OpCodes.Shl,  [Args.Mem(1), Args.Const(0)]),  // 3: $1 <<= const[0]=1
            new ScriptInstruction(OpCodes.Dec,  [Args.Mem(3)]),                 // 4: $3--
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(3), Args.Mem(4)]),    // 5: cmp $3, 0
            new ScriptInstruction(OpCodes.Jgr,  [Args.Label(3)]));              // 6: if $3 > 0 goto 3

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        // 1 << 8 = 256
        Assert.Equal(VMValue.FromInteger(1 << shifts), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressShr_ByOne_ManyTimes()
    {
        // SHR requires the shift amount as a Constant arg (not MemoryAddress).
        // Uses a countdown counter ($3) to perform exactly `shifts` right-shifts.
        const int shifts = 8;
        VMValue[] constants = [VMValue.FromInteger(1 << shifts), // const[0] = 256 (initial value)
                                VMValue.FromInteger(1),           // const[1] = 1 (shift amount)
                                VMValue.FromInteger(shifts),      // const[2] = 8 (countdown)
                                VMValue.FromInteger(0)];          // const[3] = 0 (compare target)

        Script script = Args.Build("stress_shr_many", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),  // 0: $1 = 256
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(2)]),  // 1: $3 = shifts (countdown)
            new ScriptInstruction(OpCodes.Load, [Args.Mem(4), Args.Const(3)]),  // 2: $4 = 0 (compare target)
            new ScriptInstruction(OpCodes.Shr,  [Args.Mem(1), Args.Const(1)]),  // 3: $1 >>= const[1]=1
            new ScriptInstruction(OpCodes.Dec,  [Args.Mem(3)]),                 // 4: $3--
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(3), Args.Mem(4)]),    // 5: cmp $3, 0
            new ScriptInstruction(OpCodes.Jgr,  [Args.Label(3)]));              // 6: if $3 > 0 goto 3

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        // 256 >> 8 = 1
        Assert.Equal(VMValue.FromInteger(1), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressAnd_ZeroMask_AlwaysZero()
    {
        VMValue[] constants = [VMValue.FromInteger(0xDEADBEEF), VMValue.FromInteger(0)];

        Script script = Args.Build("stress_and_zeromask", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.And,  [Args.Mem(1), Args.Mem(2)]));

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(0), executor.GetValueInMemory(1));
    }
}
