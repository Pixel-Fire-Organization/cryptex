using System.Numerics;
using Cryptex.VM.Execution;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Test.StressTests;

/// <summary>
///     Stress tests for math instructions.
///     Verifies consistent results under repeated execution and with extreme values.
/// </summary>
public sealed class MathStressTest
{
    // Runs Add 10 000 times via a VM loop and checks the final accumulated value.
    [Fact]
    [Trait("Category", "Stress")]
    public void StressAdd_ManyIterations_ConsistentResult()
    {
        const int iterations = 10_000;
        // $1 = 0 (accumulator), $2 = 1 (step), $3 = iterations
        VMValue[] constants = [VMValue.FromInteger(0), VMValue.FromInteger(1), VMValue.FromInteger(iterations)];

        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),  // 0: $1 = 0
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),  // 1: $2 = 1
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(2)]),  // 2: $3 = 10000
            new ScriptInstruction(OpCodes.Add,  [Args.Mem(1), Args.Mem(2)]),    // 3: $1 += 1  (loop body)
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(3)]),    // 4: cmp $1, $3
            new ScriptInstruction(OpCodes.Jls,  [Args.Label(3)]),               // 5: if $1 < $3 goto 3
        ]);
        Script script = new Script("stress_add", [chunk], constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(iterations), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressAdd_LargeValues_CorrectResult()
    {
        BigInteger large = BigInteger.Pow(2, 62);
        VMValue[] constants = [VMValue.FromInteger(large), VMValue.FromInteger(large)];

        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Add,  [Args.Mem(1), Args.Mem(2)])
        ]);
        Script script = new Script("stress_add_large", [chunk], constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(large + large), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressSub_ManyIterations_ConsistentResult()
    {
        const int iterations = 10_000;
        VMValue[] constants = [VMValue.FromInteger(iterations), VMValue.FromInteger(1), VMValue.FromInteger(0)];

        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),  // 0: $1 = 10000
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),  // 1: $2 = 1
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(2)]),  // 2: $3 = 0
            new ScriptInstruction(OpCodes.Sub,  [Args.Mem(1), Args.Mem(2)]),    // 3: $1 -= 1
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(3)]),    // 4: cmp $1, 0
            new ScriptInstruction(OpCodes.Jgr,  [Args.Label(3)]),               // 5: if $1 > 0 goto 3
        ]);
        Script script = new Script("stress_sub", [chunk], constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(0), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressMul_LargeValues_CorrectResult()
    {
        BigInteger large = BigInteger.Pow(2, 32);
        VMValue[] constants = [VMValue.FromInteger(large), VMValue.FromInteger(large)];

        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Mul,  [Args.Mem(1), Args.Mem(2)])
        ]);
        Script script = new Script("stress_mul_large", [chunk], constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(large * large), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressDiv_ManyIterations_ConsistentResult()
    {
        const int iterations = 1_000;
        // $1 starts at iterations * 2, $2 = 2, loop divides by 2 until result <= 1
        BigInteger start = (BigInteger)iterations * 2;
        VMValue[] constants = [VMValue.FromInteger(start), VMValue.FromInteger(2), VMValue.FromInteger(1)];

        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(3), Args.Const(2)]),
            new ScriptInstruction(OpCodes.Div,  [Args.Mem(1), Args.Mem(2)]),
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(3)]),
            new ScriptInstruction(OpCodes.Jgr,  [Args.Label(3)]),
        ]);
        Script script = new Script("stress_div", [chunk], constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.False(executor.GetValueInMemory(1).IsUndefined);
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressInc_ManyIterations_ConsistentResult()
    {
        const int iterations = 10_000;
        VMValue[] constants = [VMValue.FromInteger(0), VMValue.FromInteger(iterations)];

        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),  // 0: $1 = 0
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),  // 1: $2 = 10000
            new ScriptInstruction(OpCodes.Inc,  [Args.Mem(1)]),                 // 2: $1++
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(2)]),    // 3: cmp $1, $2
            new ScriptInstruction(OpCodes.Jls,  [Args.Label(2)]),               // 4: if $1 < $2 goto 2
        ]);
        Script script = new Script("stress_inc", [chunk], constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(iterations), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressDec_ManyIterations_ConsistentResult()
    {
        const int iterations = 10_000;
        VMValue[] constants = [VMValue.FromInteger(iterations), VMValue.FromInteger(0)];

        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),  // 0: $1 = 10000
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),  // 1: $2 = 0
            new ScriptInstruction(OpCodes.Dec,  [Args.Mem(1)]),                 // 2: $1--
            new ScriptInstruction(OpCodes.Cmp,  [Args.Mem(1), Args.Mem(2)]),    // 3: cmp $1, 0
            new ScriptInstruction(OpCodes.Jgr,  [Args.Label(2)]),               // 4: if $1 > 0 goto 2
        ]);
        Script script = new Script("stress_dec", [chunk], constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(0), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressMod_ManyValues_ConsistentResult()
    {
        const int modulus = 7;
        const int iterations = 10_000;
        VMValue[] constants = [VMValue.FromInteger(0), VMValue.FromInteger(1),
                                VMValue.FromInteger(modulus), VMValue.FromInteger(iterations)];

        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load,   [Args.Mem(1), Args.Const(0)]),  // 0: $1 = 0 (counter)
            new ScriptInstruction(OpCodes.Load,   [Args.Mem(2), Args.Const(1)]),  // 1: $2 = 1
            new ScriptInstruction(OpCodes.Load,   [Args.Mem(3), Args.Const(2)]),  // 2: $3 = modulus
            new ScriptInstruction(OpCodes.Load,   [Args.Mem(4), Args.Const(3)]),  // 3: $4 = iterations
            new ScriptInstruction(OpCodes.Add,    [Args.Mem(1), Args.Mem(2)]),    // 4: $1++
            new ScriptInstruction(OpCodes.Load,   [Args.Mem(5), Args.Const(0)]),  // 5: $5 = counter value copy setup
            new ScriptInstruction(OpCodes.Add,    [Args.Mem(5), Args.Mem(1)]),    // 6: $5 = $1 (copy via 0+$1)
            new ScriptInstruction(OpCodes.Mod,    [Args.Mem(5), Args.Mem(3)]),    // 7: $5 = $1 % modulus
            new ScriptInstruction(OpCodes.Cmp,    [Args.Mem(1), Args.Mem(4)]),    // 8: cmp $1, iterations
            new ScriptInstruction(OpCodes.Jls,    [Args.Label(4)]),               // 9: if $1 < iterations goto 4
        ]);
        Script script = new Script("stress_mod", [chunk], constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(iterations % modulus), executor.GetValueInMemory(5));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressAddF_ManyIterations_ConsistentResult()
    {
        const int iterations = 1_000;
        VMValue[] constants = [VMValue.FromFloat(0.0m), VMValue.FromFloat(0.1m),
                                VMValue.FromFloat((decimal)iterations)];

        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load,  [Args.Mem(1), Args.Const(0)]),  // 0: $1 = 0.0
            new ScriptInstruction(OpCodes.Load,  [Args.Mem(2), Args.Const(1)]),  // 1: $2 = 0.1
            new ScriptInstruction(OpCodes.Load,  [Args.Mem(3), Args.Const(2)]),  // 2: $3 = 1000.0
            new ScriptInstruction(OpCodes.AddF,  [Args.Mem(1), Args.Mem(2)]),    // 3: $1 += 0.1
            new ScriptInstruction(OpCodes.Cmp,   [Args.Mem(1), Args.Mem(3)]),    // 4: cmp $1, $3
            new ScriptInstruction(OpCodes.Jls,   [Args.Label(3)]),               // 5: if $1 < $3 goto 3
        ]);
        Script script = new Script("stress_addf", [chunk], constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.False(executor.GetValueInMemory(1).IsUndefined);
        Assert.True(executor.GetValueInMemory(1).IsFloat);
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressAddImm_ManyIterations_ConsistentResult()
    {
        const int iterations = 10_000;
        const int step = 3;
        VMValue[] constants = [VMValue.FromInteger(0), VMValue.FromInteger(step),
                                VMValue.FromInteger(iterations * step)];

        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load,   [Args.Mem(1), Args.Const(0)]),      // 0: $1 = 0
            new ScriptInstruction(OpCodes.Load,   [Args.Mem(2), Args.Const(2)]),      // 1: $2 = target
            new ScriptInstruction(OpCodes.AddImm, [Args.Mem(1), Args.Const(1)]),      // 2: $1 += step
            new ScriptInstruction(OpCodes.Cmp,    [Args.Mem(1), Args.Mem(2)]),        // 3: cmp $1, target
            new ScriptInstruction(OpCodes.Jls,    [Args.Label(2)]),                   // 4: if $1 < target goto 2
        ]);
        Script script = new Script("stress_addimm", [chunk], constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(iterations * step), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressAdd_NegativeValues_ConsistentResult()
    {
        BigInteger large = -BigInteger.Pow(2, 62);
        VMValue[] constants = [VMValue.FromInteger(large), VMValue.FromInteger(1)];

        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
            new ScriptInstruction(OpCodes.Add,  [Args.Mem(1), Args.Mem(2)])
        ]);
        Script script = new Script("stress_add_negative", [chunk], constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(large + 1), executor.GetValueInMemory(1));
    }

    [Fact]
    [Trait("Category", "Stress")]
    public void StressMulImm_LargeConstant_CorrectResult()
    {
        const long value = 1_000_000L;
        const long factor = 1_000L;
        VMValue[] constants = [VMValue.FromInteger(value), VMValue.FromInteger(factor)];

        ScriptChunk chunk = new ScriptChunk("main", [
            new ScriptInstruction(OpCodes.Load,   [Args.Mem(1), Args.Const(0)]),
            new ScriptInstruction(OpCodes.MulImm, [Args.Mem(1), Args.Const(1)])
        ]);
        Script script = new Script("stress_mulimm_large", [chunk], constants);

        Executor executor = new Executor(script);
        Assert.True(executor.ExecuteScript());

        Assert.Equal(VMValue.FromInteger(value * factor), executor.GetValueInMemory(1));
    }
}
