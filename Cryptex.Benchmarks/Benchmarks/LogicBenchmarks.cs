using static Cryptex.Benchmarks.ScriptRunner;

namespace Cryptex.Benchmarks.Benchmarks;

/// <summary>
///     Benchmarks for logic instructions: Cmp, Jmp, Jeq, Jnq, Jls, Jgr, Jge, Jle.
///     Each benchmark executes a minimal script that exercises the target instruction once.
/// </summary>
[MemoryDiagnoser]
public class LogicBenchmarks
{
    private static readonly VMValue[] Constants =
    [
        VMValue.FromInteger(5),  // const[0]
        VMValue.FromInteger(10), // const[1]
        VMValue.FromInteger(0)   // const[2] — Nop sleep ms (0)
    ];

    private Script m_cmpScript = null!;
    private Script m_jmpScript = null!;
    private Script m_jeqScript = null!;
    private Script m_jnqScript = null!;
    private Script m_jlsScript = null!;
    private Script m_jgrScript = null!;
    private Script m_jgeScript = null!;
    private Script m_jleScript = null!;

    [GlobalSetup]
    public void Setup()
    {
        // cmp: compare two values (sets flag, no jump)
        m_cmpScript = Build("cmp", Constants,
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Mem(2), Const(1)]),
            new ScriptInstruction(OpCodes.Cmp,  [Mem(1), Mem(2)]));

        // jmp: unconditional jump over a Load instruction, then a Nop (sleep 0ms) to end.
        m_jmpScript = Build("jmp", Constants,
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.Jmp,  [Label(3)]),              // 1 → skip 2
            new ScriptInstruction(OpCodes.Load, [Mem(2), Const(1)]),      // 2 (skipped)
            new ScriptInstruction(OpCodes.Nop,  [Const(2)]));             // 3 (sleep 0ms)

        // jeq: cmp two equal values → flag=Equals → Jeq taken → Nop
        m_jeqScript = Build("jeq", Constants,
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Mem(2), Const(0)]),
            new ScriptInstruction(OpCodes.Cmp,  [Mem(1), Mem(2)]),
            new ScriptInstruction(OpCodes.Jeq,  [Label(5)]),              // 3 → skip 4
            new ScriptInstruction(OpCodes.Load, [Mem(3), Const(1)]),      // 4 (skipped)
            new ScriptInstruction(OpCodes.Nop,  [Const(2)]));             // 5

        // jnq: cmp two different values → flag=Less → Jnq taken → Nop
        m_jnqScript = Build("jnq", Constants,
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Mem(2), Const(1)]),
            new ScriptInstruction(OpCodes.Cmp,  [Mem(1), Mem(2)]),
            new ScriptInstruction(OpCodes.Jnq,  [Label(5)]),
            new ScriptInstruction(OpCodes.Load, [Mem(3), Const(0)]),      // skipped
            new ScriptInstruction(OpCodes.Nop,  [Const(2)]));

        // jls: 5 < 10 → Less → Jls taken
        m_jlsScript = Build("jls", Constants,
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Mem(2), Const(1)]),
            new ScriptInstruction(OpCodes.Cmp,  [Mem(1), Mem(2)]),
            new ScriptInstruction(OpCodes.Jls,  [Label(5)]),
            new ScriptInstruction(OpCodes.Load, [Mem(3), Const(0)]),      // skipped
            new ScriptInstruction(OpCodes.Nop,  [Const(2)]));

        // jgr: 10 > 5 → Greater → Jgr taken
        m_jgrScript = Build("jgr", Constants,
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(1)]),
            new ScriptInstruction(OpCodes.Load, [Mem(2), Const(0)]),
            new ScriptInstruction(OpCodes.Cmp,  [Mem(1), Mem(2)]),
            new ScriptInstruction(OpCodes.Jgr,  [Label(5)]),
            new ScriptInstruction(OpCodes.Load, [Mem(3), Const(0)]),      // skipped
            new ScriptInstruction(OpCodes.Nop,  [Const(2)]));

        // jge: 10 >= 5 → Greater → Jge taken
        m_jgeScript = Build("jge", Constants,
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(1)]),
            new ScriptInstruction(OpCodes.Load, [Mem(2), Const(0)]),
            new ScriptInstruction(OpCodes.Cmp,  [Mem(1), Mem(2)]),
            new ScriptInstruction(OpCodes.Jge,  [Label(5)]),
            new ScriptInstruction(OpCodes.Load, [Mem(3), Const(0)]),      // skipped
            new ScriptInstruction(OpCodes.Nop,  [Const(2)]));

        // jle: 5 <= 10 → Less → Jle taken
        m_jleScript = Build("jle", Constants,
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Mem(2), Const(1)]),
            new ScriptInstruction(OpCodes.Cmp,  [Mem(1), Mem(2)]),
            new ScriptInstruction(OpCodes.Jle,  [Label(5)]),
            new ScriptInstruction(OpCodes.Load, [Mem(3), Const(0)]),      // skipped
            new ScriptInstruction(OpCodes.Nop,  [Const(2)]));
    }

    [Benchmark] public bool Cmp() => Run(m_cmpScript);
    [Benchmark] public bool Jmp() => Run(m_jmpScript);
    [Benchmark] public bool Jeq() => Run(m_jeqScript);
    [Benchmark] public bool Jnq() => Run(m_jnqScript);
    [Benchmark] public bool Jls() => Run(m_jlsScript);
    [Benchmark] public bool Jgr() => Run(m_jgrScript);
    [Benchmark] public bool Jge() => Run(m_jgeScript);
    [Benchmark] public bool Jle() => Run(m_jleScript);
}
