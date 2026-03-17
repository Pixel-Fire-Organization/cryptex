using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.Test.StressTests;

/// <summary>
///     Stress tests for logic/jump instructions.
///     Verifies that conditional jumps and compare flags behave consistently
///     across many loop iterations and all comparison outcomes.
/// </summary>
public sealed class LogicStressTest
{
    [Fact]
    [Trait("Category", "Stress")]
    public void StressJmp_ManyIterations_LoopTerminates()
    {
        const int iterations = 10_000;
        // const[0]=0 serves as both counter start and Nop sleep-ms (0 ms).
        VMValue[] constants = [VMValue.FromInteger(0), VMValue.FromInteger(1), VMValue.FromInteger(iterations)];

        // Pattern: increment counter, compare; Jeq exits to Nop when done, Jmp loops back.
        Script script = Args.Build("stress_jmp_loop", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),  // 0: $1 = 0
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),  // 1: $2 = 1
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(2)]),  // 2: $3 = iterations
            new ScriptInstruction(OpCodes.Add,  [Args.Mem(1), Args.Mem(2)]),    // 3: $1 += 1  (loop body)
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(3)]),    // 4: cmp $1, iterations
            new ScriptInstruction(OpCodes.Jeq,  [Args.Label(7)]),               // 5: if equal goto 7
            new ScriptInstruction(OpCodes.Jmp,  [Args.Label(3)]),               // 6: goto 3
            new ScriptInstruction(OpCodes.Nop,  [Args.Const(0)]));              // 7: nop (sleep 0ms) — loop exit

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(iterations), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressJeq_EqualBranch_AlwaysTaken()
    {
        const int iterations = 5_000;
        VMValue[] constants = [VMValue.FromInteger(42), VMValue.FromInteger(42),
                                VMValue.FromInteger(0), VMValue.FromInteger(iterations)];

        // Each iteration: cmp 42 == 42 → jeq taken, accumulate.
        Script script = Args.Build("stress_jeq_always", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),  // 0: $1 = 42 (a)
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),  // 1: $2 = 42 (b)
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(2)]),  // 2: $3 = 0 (branch-taken counter)
            new ScriptInstruction(OpCodes.Load, [Args.Mem(4), Args.Const(2)]),  // 3: $4 = 0 (loop counter)
            new ScriptInstruction(OpCodes.Load, [Args.Mem(5), Args.Const(3)]),  // 4: $5 = iterations
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(2)]),    // 5: cmp 42, 42
            new ScriptInstruction(OpCodes.Jeq,  [Args.Label(8)]),               // 6: if equal goto 8
            new ScriptInstruction(OpCodes.Jmp,  [Args.Label(9)]),               // 7: (not-equal path — should not be reached)
            new ScriptInstruction(OpCodes.Inc,  [Args.Mem(3)]),                 // 8: $3++ (branch-taken counter)
            new ScriptInstruction(OpCodes.Inc,  [Args.Mem(4)]),                 // 9: $4++
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(4), Args.Mem(5)]),   // 10: cmp $4, iterations
            new ScriptInstruction(OpCodes.Jls,  [Args.Label(5)]));              // 11: if $4 < iterations goto 5

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        // Every iteration should have taken the Jeq branch.
        Assert.Equal(VMValue.FromInteger(iterations), executor.GetValueInMemory(3));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressJnq_NotEqualBranch_AlwaysTaken()
    {
        const int iterations = 5_000;
        VMValue[] constants = [VMValue.FromInteger(1), VMValue.FromInteger(2),
                                VMValue.FromInteger(0), VMValue.FromInteger(iterations)];

        Script script = Args.Build("stress_jnq_always", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),  // 0: $1 = 1
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),  // 1: $2 = 2
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(2)]),  // 2: $3 = 0 (taken counter)
            new ScriptInstruction(OpCodes.Load, [Args.Mem(4), Args.Const(2)]),  // 3: $4 = 0 (loop counter)
            new ScriptInstruction(OpCodes.Load, [Args.Mem(5), Args.Const(3)]),  // 4: $5 = iterations
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(2)]),    // 5: cmp 1, 2
            new ScriptInstruction(OpCodes.Jnq,  [Args.Label(8)]),               // 6: if not-equal goto 8
            new ScriptInstruction(OpCodes.Jmp,  [Args.Label(9)]),               // 7: equal path — must never run
            new ScriptInstruction(OpCodes.Inc,  [Args.Mem(3)]),                 // 8: $3++
            new ScriptInstruction(OpCodes.Inc,  [Args.Mem(4)]),                 // 9: $4++
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(4), Args.Mem(5)]),   // 10: cmp $4, iterations
            new ScriptInstruction(OpCodes.Jls,  [Args.Label(5)]));              // 11: if $4 < iterations goto 5

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(iterations), executor.GetValueInMemory(3));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressJls_LessThan_CorrectLoopCount()
    {
        const int limit = 7_500;
        VMValue[] constants = [VMValue.FromInteger(0), VMValue.FromInteger(1), VMValue.FromInteger(limit)];

        Script script = Args.Build("stress_jls_loop", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),  // 0: $1 = 0
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),  // 1: $2 = 1
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(2)]),  // 2: $3 = limit
            new ScriptInstruction(OpCodes.Inc,  [Args.Mem(1)]),                 // 3: $1++
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(3)]),    // 4: cmp $1, limit
            new ScriptInstruction(OpCodes.Jls,  [Args.Label(3)]));              // 5: if $1 < limit goto 3

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(limit), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressJgr_GreaterThan_CorrectLoopCount()
    {
        const int start = 10_000;
        VMValue[] constants = [VMValue.FromInteger(start), VMValue.FromInteger(0)];

        Script script = Args.Build("stress_jgr_loop", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),  // 0: $1 = start
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),  // 1: $2 = 0
            new ScriptInstruction(OpCodes.Dec,  [Args.Mem(1)]),                 // 2: $1--
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(2)]),    // 3: cmp $1, 0
            new ScriptInstruction(OpCodes.Jgr,  [Args.Label(2)]));              // 4: if $1 > 0 goto 2

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(0), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressJge_GreaterOrEqual_TerminatesAtZero()
    {
        const int start = 5_000;
        VMValue[] constants = [VMValue.FromInteger(start), VMValue.FromInteger(0)];

        Script script = Args.Build("stress_jge_loop", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),  // 0: $1 = start
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),  // 1: $2 = 0
            new ScriptInstruction(OpCodes.Dec,  [Args.Mem(1)]),                 // 2: $1--
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(2)]),    // 3: cmp $1, 0
            new ScriptInstruction(OpCodes.Jge,  [Args.Label(2)]));              // 4: if $1 >= 0 goto 2

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(-1), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressJle_LessOrEqual_TerminatesAtLimit()
    {
        const int limit = 5_000;
        VMValue[] constants = [VMValue.FromInteger(0), VMValue.FromInteger(limit)];

        Script script = Args.Build("stress_jle_loop", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),  // 0: $1 = 0
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),  // 1: $2 = limit
            new ScriptInstruction(OpCodes.Inc,  [Args.Mem(1)]),                 // 2: $1++
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(2)]),    // 3: cmp $1, limit
            new ScriptInstruction(OpCodes.Jle,  [Args.Label(2)]));              // 4: if $1 <= limit goto 2

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(limit + 1), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressCmp_AllOutcomes_ConsistentFlags()
    {
        // Tests Equals, Greater, and Less outcomes across separate executions.
        // const[2]=0 is reused as the Nop sleep-ms argument (0ms).
        VMValue[] constants = [VMValue.FromInteger(5), VMValue.FromInteger(10),
                                VMValue.FromInteger(0), VMValue.FromInteger(1)];

        // Equals path
        Script scriptEq = Args.Build("cmp_eq", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(2)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(4), Args.Const(3)]),
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(2)]),
            new ScriptInstruction(OpCodes.Jeq,  [Args.Label(7)]),
            new ScriptInstruction(OpCodes.Jmp,  [Args.Label(8)]),
            new ScriptInstruction(OpCodes.Add,  [Args.Mem(3), Args.Mem(4)]),  // 7: taken = 1
            new ScriptInstruction(OpCodes.Nop,  [Args.Const(2)]));            // 8: nop (sleep 0ms)

        Executor exEq = new Executor(scriptEq);
        Assert.True(exEq.ExecuteScript());
        Assert.Equal(VMValue.FromInteger(1), exEq.GetValueInMemory(3));

        // Less path
        Script scriptLs = Args.Build("cmp_ls", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(2)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(4), Args.Const(3)]),
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(2)]),
            new ScriptInstruction(OpCodes.Jls,  [Args.Label(7)]),
            new ScriptInstruction(OpCodes.Jmp,  [Args.Label(8)]),
            new ScriptInstruction(OpCodes.Add,  [Args.Mem(3), Args.Mem(4)]),
            new ScriptInstruction(OpCodes.Nop,  [Args.Const(2)]));

        Executor exLs = new Executor(scriptLs);
        Assert.True(exLs.ExecuteScript());
        Assert.Equal(VMValue.FromInteger(1), exLs.GetValueInMemory(3));

        // Greater path
        Script scriptGr = Args.Build("cmp_gr", constants,
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(2)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(4), Args.Const(3)]),
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(2)]),
            new ScriptInstruction(OpCodes.Jgr,  [Args.Label(7)]),
            new ScriptInstruction(OpCodes.Jmp,  [Args.Label(8)]),
            new ScriptInstruction(OpCodes.Add,  [Args.Mem(3), Args.Mem(4)]),
            new ScriptInstruction(OpCodes.Nop,  [Args.Const(2)]));

        Executor exGr = new Executor(scriptGr);
        Assert.True(exGr.ExecuteScript());
        Assert.Equal(VMValue.FromInteger(1), exGr.GetValueInMemory(3));
    }
}
